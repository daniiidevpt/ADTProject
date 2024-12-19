using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform weaponPosition;

    private void Start()
    {
        if (weaponPosition == null)
        {
            Debug.LogError("PlayerAttack: WeaponPosition is not assigned!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attacking");
        }
    }
}
