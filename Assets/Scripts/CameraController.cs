using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // The player or object the camera will orbit
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5); // Default camera offset

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f; // Speed of camera rotation
    [SerializeField] private float minVerticalAngle = -30f; // Minimum vertical angle (looking down)
    [SerializeField] private float maxVerticalAngle = 60f;  // Maximum vertical angle (looking up)

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f; // Speed of zooming
    [SerializeField] private float minZoomDistance = 2f; // Minimum zoom distance
    [SerializeField] private float maxZoomDistance = 10f; // Maximum zoom distance

    private float currentZoomDistance; // Current distance from the target
    private float pitch = 0f; // Vertical rotation
    private float yaw = 0f;   // Horizontal rotation

    private void Start()
    {
        // Initialize zoom distance based on the offset
        currentZoomDistance = offset.magnitude;

        // Initialize rotation angles
        Vector3 direction = offset.normalized;
        pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(direction.x, -direction.z) * Mathf.Rad2Deg;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraController: No target assigned.");
            return;
        }

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

        // Clamp pitch to prevent flipping
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
        // Calculate camera offset based on pitch and yaw
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * currentZoomDistance) + Vector3.up * offset.y;

        // Set the camera's position and rotation
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}
