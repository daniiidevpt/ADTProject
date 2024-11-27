using UnityEditor;
using UnityEngine;

public class WeaponPartCreatorEditor : EditorWindow
{
    private string partName = "New Part";
    private WeaponPartType partType = WeaponPartType.Blade;
    private Mesh mesh;

    [MenuItem("Weapons/Weapon Part Creator")]
    public static void OpenWindow()
    {
        GetWindow<WeaponPartCreatorEditor>("Weapon Part Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create a New Weapon Part", EditorStyles.boldLabel);

        partName = EditorGUILayout.TextField("Part Name", partName);
        partType = (WeaponPartType)EditorGUILayout.EnumPopup("Part Type", partType);
        mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", mesh, typeof(Mesh), false);

        if (GUILayout.Button("Create Part"))
        {
            CreateWeaponPart();
        }
    }

    private void CreateWeaponPart()
    {
        if (mesh == null)
        {
            Debug.LogError("Please assign a mesh before creating a part.");
            return;
        }

        WeaponPart part = CreateInstance<WeaponPart>();
        part.name = partName;
        part.partType = partType;

        part.vertices = mesh.vertices;
        part.triangles = mesh.triangles;
        part.uvs = mesh.uv;

        string path = $"Assets/{partName}.asset";
        AssetDatabase.CreateAsset(part, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"Weapon Part '{partName}' created at {path}");
    }
}
