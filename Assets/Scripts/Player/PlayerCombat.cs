using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f; // Cooldown between attacks
    private float attackCooldownTimer = 0f;

    private Animator animator;

    public bool IsAttacking { get; private set; } = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void HandleAttack()
    {
        attackCooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && attackCooldownTimer >= attackCooldown)
        {
            PerformRandomAttack();
            attackCooldownTimer = 0; // Reset cooldown
        }

        UpdateAttackState();
    }

    private void PerformRandomAttack()
    {
        // Randomly choose between Attack1 and Attack2
        int attackIndex = Random.Range(1, 3); // Random value between 1 (inclusive) and 3 (exclusive)
        string attackTrigger = attackIndex == 1 ? "Attack1" : "Attack2";

        animator.SetTrigger(attackTrigger);
        IsAttacking = true; // Mark as attacking
    }

    private void UpdateAttackState()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // Update IsAttacking based on current animation state
        IsAttacking = currentState.IsName("Attack1") || currentState.IsName("Attack2");
    }

    public bool IsAttackAnimationPlaying()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName("Attack1") || currentState.IsName("Attack2");
    }
}
