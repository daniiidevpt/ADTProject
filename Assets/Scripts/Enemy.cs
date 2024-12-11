using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}

