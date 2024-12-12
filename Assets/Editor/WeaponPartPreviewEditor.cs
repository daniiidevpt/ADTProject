using UnityEditor;
using UnityEngine;

public class WeaponPartPreviewEditor : EditorWindow
{
    private WeaponPart weaponPart; // Reference to the WeaponPart scriptable object
    private Mesh previewMesh; // The generated preview mesh

    [MenuItem("Weapons/Weapon Part Preview")]
    public static void OpenWindow()
    {
        WeaponPartPreviewEditor window = GetWindow<WeaponPartPreviewEditor>("Weapon Part Preview");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Part Preview", EditorStyles.boldLabel);

        weaponPart = (WeaponPart)EditorGUILayout.ObjectField("Weapon Part", weaponPart, typeof(WeaponPart), false);

        if (weaponPart == null)
        {
            EditorGUILayout.HelpBox("Assign a WeaponPart asset to preview the part.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Generate Preview"))
        {
            GeneratePreviewMesh();
        }

        if (previewMesh != null)
        {
            GUILayout.Label("Preview Generated. Check Scene View.");
        }
    }

    private void GeneratePreviewMesh()
    {
        if (weaponPart == null) return;

        // Create a new mesh for preview
        previewMesh = new Mesh { name = $"{weaponPart.partName} Preview" };

        previewMesh.vertices = weaponPart.vertices;
        previewMesh.triangles = weaponPart.triangles;
        previewMesh.uv = weaponPart.uvs;

        previewMesh.RecalculateNormals();
        previewMesh.RecalculateBounds();

        // Assign preview mesh to a temporary GameObject
        GameObject previewObject = GameObject.Find("Weapon Part Preview Object");
        if (previewObject == null)
        {
            previewObject = new GameObject("Weapon Part Preview Object");
            previewObject.hideFlags = HideFlags.DontSave;
            previewObject.AddComponent<MeshFilter>().mesh = previewMesh;
            previewObject.AddComponent<MeshRenderer>();
        }
        else
        {
            previewObject.GetComponent<MeshFilter>().mesh = previewMesh;
        }

        // Assign a default material or use an existing one
        Material defaultMaterial = new Material(Shader.Find("Standard"));
        previewObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
    }

    private void OnDisable()
    {
        // Clean up the preview object when the editor window is closed
        GameObject previewObject = GameObject.Find("Weapon Part Preview Object");
        if (previewObject != null)
        {
            DestroyImmediate(previewObject);
        }
    }
}
