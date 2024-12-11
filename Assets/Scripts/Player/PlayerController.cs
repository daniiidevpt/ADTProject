using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (playerCombat.IsAttacking)
        {
            playerMovement.CanMove = false;
        }
        else
        {
            playerMovement.CanMove = true;
        }

        playerMovement.HandleMovement();
        playerMovement.HandleGravity();
        playerCombat.HandleAttack();
    }
}
