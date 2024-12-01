using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    private Animator animator;
    private PlayerCombat playerCombat;

    private Vector3 velocity;
    private float horizontalInput;
    private float verticalInput;

    public bool CanMove { get; set; } = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public void HandleMovement()
    {
        if (!CanMove || playerCombat.IsAttackAnimationPlaying()) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 normalizedDirection = moveDirection.normalized;

        characterController.Move(normalizedDirection * walkSpeed * Time.deltaTime);

        float velocityMagnitude = new Vector3(horizontalInput, 0, verticalInput).magnitude;
        animator.SetFloat("Velocity", velocityMagnitude);
    }

    public void HandleGravity()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
