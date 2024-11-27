using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class WeaponPreviewEditor : EditorWindow
{
    //private ModularWeaponMorph weaponMorph; // Reference to the ModularWeaponMorph component
    //private Mesh previewMesh; // The generated preview mesh

    //[MenuItem("Weapons/Weapon Preview")]
    //public static void OpenWindow()
    //{
    //    WeaponPreviewEditor window = GetWindow<WeaponPreviewEditor>("Weapon Preview");
    //    window.Show();
    //}

    //private void OnGUI()
    //{
    //    GUILayout.Label("Modular Weapon Preview", EditorStyles.boldLabel);

    //    weaponMorph = (ModularWeaponMorph)EditorGUILayout.ObjectField("Weapon Morph Script", weaponMorph, typeof(ModularWeaponMorph), true);

    //    if (weaponMorph == null)
    //    {
    //        EditorGUILayout.HelpBox("Assign a ModularWeaponMorph script to preview the weapon.", MessageType.Info);
    //        return;
    //    }

    //    if (GUILayout.Button("Generate Preview"))
    //    {
    //        GeneratePreviewMesh();
    //    }

    //    if (previewMesh != null)
    //    {
    //        GUILayout.Label("Preview Generated. Check Scene View.");
    //    }
    //}

    //private void GeneratePreviewMesh()
    //{
    //    if (weaponMorph == null) return;

    //    // Create a new mesh for preview
    //    previewMesh = new Mesh { name = "Weapon Preview" };

    //    List<Vector3> allVertices = new List<Vector3>();
    //    List<int> allTriangles = new List<int>();
    //    List<Vector2> allUVs = new List<Vector2>();

    //    int vertexOffset = 0;

    //    foreach (var part in weaponMorph.weaponParts)
    //    {
    //        if (part.sourcePart == null) continue;

    //        // Apply alignment transform to source vertices
    //        Vector3[] sourceVertices = ApplyTransform(part.sourcePart.vertices, part.alignmentTransform);

    //        allVertices.AddRange(sourceVertices);
    //        allUVs.AddRange(part.sourcePart.uvs);

    //        foreach (int tri in part.sourcePart.triangles)
    //        {
    //            allTriangles.Add(tri + vertexOffset);
    //        }

    //        vertexOffset += sourceVertices.Length;
    //    }

    //    previewMesh.vertices = allVertices.ToArray();
    //    previewMesh.triangles = allTriangles.ToArray();
    //    previewMesh.uv = allUVs.ToArray();

    //    previewMesh.RecalculateNormals();
    //    previewMesh.RecalculateBounds();

    //    // Assign preview mesh to a temporary GameObject
    //    GameObject previewObject = GameObject.Find("Weapon Preview Object");
    //    if (previewObject == null)
    //    {
    //        previewObject = new GameObject("Weapon Preview Object");
    //        previewObject.hideFlags = HideFlags.DontSave;
    //        previewObject.AddComponent<MeshFilter>().mesh = previewMesh;
    //        previewObject.AddComponent<MeshRenderer>();
    //    }
    //    else
    //    {
    //        previewObject.GetComponent<MeshFilter>().mesh = previewMesh;
    //    }

    //    // Use the same material as the weapon morph object, if available
    //    if (weaponMorph.GetComponent<MeshRenderer>() != null)
    //    {
    //        previewObject.GetComponent<MeshRenderer>().sharedMaterial = weaponMorph.GetComponent<MeshRenderer>().sharedMaterial;
    //    }
    //}

    //private Vector3[] ApplyTransform(Vector3[] vertices, Transform transform)
    //{
    //    if (transform == null) return vertices;

    //    Vector3[] transformedVertices = new Vector3[vertices.Length];
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        transformedVertices[i] = transform.TransformPoint(vertices[i]);
    //    }
    //    return transformedVertices;
    //}


    //private void OnDisable()
    //{
    //    // Clean up the preview object when the editor window is closed
    //    GameObject previewObject = GameObject.Find("Weapon Preview Object");
    //    if (previewObject != null)
    //    {
    //        DestroyImmediate(previewObject);
    //    }
    //}
}
