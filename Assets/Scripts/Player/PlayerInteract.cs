using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform;

    private ArmoryShowcase currentArmoryShowcase;

    private void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("PlayerInteract: CameraTransform is not assigned!");
        }
    }

    private void Update()
    {
        HandleInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentArmoryShowcase != null)
        {
            GameManager.Instance.MorphInto(currentArmoryShowcase.BladeMesh);
        }
    }

    private void HandleInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            ArmoryShowcase detectedArmoryShowcase = hit.collider.GetComponent<ArmoryShowcase>();
            if (detectedArmoryShowcase != null)
            {
                if (detectedArmoryShowcase != currentArmoryShowcase)
                {
                    if (currentArmoryShowcase != null)
                    {
                        currentArmoryShowcase.UnselectWeapon();
                    }

                    currentArmoryShowcase = detectedArmoryShowcase;
                    currentArmoryShowcase.SelectWeapon();
                }
            }
        }
        else
        {
            if (currentArmoryShowcase != null)
            {
                currentArmoryShowcase.UnselectWeapon();
                currentArmoryShowcase = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (cameraTransform == null) return;

        Gizmos.color = currentArmoryShowcase != null ? Color.green : Color.red;

        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * interactRange);
        Gizmos.DrawWireSphere(cameraTransform.position + cameraTransform.forward * interactRange, 0.1f);
    }
}
