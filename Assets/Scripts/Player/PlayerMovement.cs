using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;

    [Header("Rotation Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Vector3 movementInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            Debug.LogError("PlayerMovement: CameraTransform is not assigned!");
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Determine movement direction relative to the camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Flatten the directions on the Y-axis to prevent unwanted vertical movement
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        movementInput = forward * moveZ + right * moveX;
    }

    private void HandleMovement()
    {
        Vector3 moveForce = movementInput.normalized * movementSpeed;
        rb.velocity = new Vector3(moveForce.x, rb.velocity.y, moveForce.z);
    }

    private void HandleRotation()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            rb.MoveRotation(targetRotation);
        }
    }
}
