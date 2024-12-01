using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Attack Settings")]
    [SerializeField] private float comboResetTime = 1f;
    [SerializeField] private float attackCooldown = 0.5f;
    public bool isAttacking = false;
    public float comboTimer = 0f;
    public int comboStage = 0;
    public float attackCooldownTimer = 0f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    private CharacterController characterController;
    private Animator animator;

    private Vector3 velocity;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 normalizedDirection = moveDirection.normalized;

        characterController.Move(normalizedDirection * walkSpeed * Time.deltaTime);

        float velocity = new Vector3(horizontalInput, 0, verticalInput).magnitude;
        animator.SetFloat("Velocity", velocity);
    }

    private void HandleAttack()
    {
        attackCooldownTimer -= Time.deltaTime; // Decrement cooldown timer

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // Check for attack input
        if (Input.GetButtonDown("Fire1") && attackCooldownTimer <= 0)
        {
            if (!isAttacking) // First attack
            {
                comboStage = 1;
                animator.SetTrigger("Attack1");
                isAttacking = true;
                comboTimer = comboResetTime; // Start combo timer
            }
            else if (comboStage == 1 && currentState.IsName("Attack1") && currentState.normalizedTime >= 0.5f)
            {
                // Second attack when the first attack is at least halfway done
                comboStage = 2;
                animator.SetTrigger("Attack2");
                comboTimer = comboResetTime; // Restart combo timer
            }

            attackCooldownTimer = attackCooldown; // Start cooldown
        }

        // Check if current animation is finished
        if (isAttacking && currentState.normalizedTime >= 1.0f)
        {
            if (comboStage == 2) // If on the second attack, reset
            {
                comboStage = 0;
                isAttacking = false;
            }
        }

        // Reset combo if the timer runs out
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                comboStage = 0;
                isAttacking = false;
            }
        }
    }

    private void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
