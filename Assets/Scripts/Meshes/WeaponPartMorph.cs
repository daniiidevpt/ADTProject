using UnityEngine;

[System.Serializable]
public class WeaponPartMorph
{
    [Header("General Settings")]
    public WeaponPart sourcePart = null;
    public WeaponPart targetPart = null;

    [Header("Morph Settings")]
    public float morphDuration = 2f;
    public bool useDynamicMapping = true;

    [Header("Debug Settings")]
    public bool showDebug = false;
    [Space(10)]
    public bool showVertexMapping = false;
    public bool showBoundingBoxes = false;
    public bool showPivotPoints = false;
}
