using UnityEngine;

public enum WeaponPartType
{
    Blade,
    Hilt,
    Guard
}

[CreateAssetMenu(fileName = "WeaponPart", menuName = "ModularWeapon/WeaponPart")]
public class WeaponPart : ScriptableObject
{
    public string partName;
    public WeaponPartType partType;

    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    public Vector3 offset = Vector3.zero;
}
