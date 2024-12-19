using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform firstPersonTarget;
    [SerializeField] private Vector3 thirdPersonOffset = new Vector3(0, 2, -5);

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float minVerticalAngleFirstPerson;
    [SerializeField] private float maxVerticalAngleFirstPerson;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoomDistance;
    [SerializeField] private float maxZoomDistance;

    [Header("Camera Mode")]
    [SerializeField] private bool isFirstPerson = false;

    private float currentZoomDistance;
    private float pitch = 0f;
    private float yaw = 0f;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraController: Target is not assigned in the inspector!");
            return;
        }

        if (firstPersonTarget == null && isFirstPerson)
        {
            Debug.LogError("CameraController: First Person Target is not assigned in the inspector!");
            return;
        }

        currentZoomDistance = thirdPersonOffset.magnitude;

        // Nice calculations
        Vector3 direction = thirdPersonOffset.normalized;
        pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(direction.x, -direction.z) * Mathf.Rad2Deg;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // TODO: Remove after demo is finished
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson && !firstPersonTarget)
            {
                Debug.LogWarning("First Person Target is not assigned! Defaulting to third person.");
                isFirstPerson = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (isFirstPerson)
        {
            HandleFirstPersonView();
        }
        else
        {
            HandleThirdPersonView();
        }
    }

    private void HandleFirstPersonView()
    {
        if (firstPersonTarget == null)
        {
            Debug.LogWarning("First Person Target is missing. Ensure it's assigned in the inspector.");
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minVerticalAngleFirstPerson, maxVerticalAngleFirstPerson);

        transform.position = firstPersonTarget.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void HandleThirdPersonView()
    {
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoomDistance -= scrollInput * zoomSpeed;
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * currentZoomDistance) + Vector3.up * thirdPersonOffset.y;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * thirdPersonOffset.y);
    }
}
