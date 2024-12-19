using UnityEngine;

public class ArmoryShowcase : MonoBehaviour
{
    [Header("Showcase Settings")]
    [SerializeField] private Transform bladeShowcase;
    [SerializeField] private Material selectedMaterial;
    private Material cachedMaterial;

    private Mesh bladeMesh;

    public Mesh BladeMesh { get => bladeMesh; set => bladeMesh = value; }

    public void SelectWeapon()
    {
        MeshRenderer meshRenderer = bladeShowcase.GetChild(0).GetComponent<MeshRenderer>();
        MeshFilter meshFilter = bladeShowcase.GetChild(0).GetComponent<MeshFilter>();

        cachedMaterial = meshRenderer.material;
        meshRenderer.sharedMaterial = selectedMaterial;

        bladeMesh = meshFilter.sharedMesh;
    }

    public void UnselectWeapon()
    {
        MeshRenderer meshRenderer = bladeShowcase.GetChild(0).GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = cachedMaterial;

        bladeMesh = null;
    }
}
