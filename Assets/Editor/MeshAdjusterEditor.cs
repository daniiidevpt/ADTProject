using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshAdjuster))]
public class MeshAdjusterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshAdjuster adjuster = (MeshAdjuster)target;

        EditorGUILayout.LabelField("Mesh Adjuster", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        adjuster.sourcePart = (WeaponPart)EditorGUILayout.ObjectField("Source Part", adjuster.sourcePart, typeof(WeaponPart), false);
        adjuster.targetPart = (WeaponPart)EditorGUILayout.ObjectField("Target Part", adjuster.targetPart, typeof(WeaponPart), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        adjuster.adjustToTargetVertexCount = EditorGUILayout.Toggle("Adjust to Target Vertex Count", adjuster.adjustToTargetVertexCount);
        adjuster.enableGizmos = EditorGUILayout.Toggle("Enable Gizmos", adjuster.enableGizmos);
        adjuster.visualizationStepDuration = EditorGUILayout.FloatField("Step Duration", adjuster.visualizationStepDuration);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
        EditorGUILayout.IntField("Source Vertex Count", adjuster.sourceVertexCount);
        EditorGUILayout.IntField("Target Vertex Count", adjuster.targetVertexCount);
        EditorGUILayout.IntField("Adjusted Vertex Count", adjuster.adjustedVertexCount);

        EditorGUILayout.Space();
        if (GUILayout.Button("Adjust and Visualize"))
        {
            adjuster.AdjustMesh();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(adjuster);
        }
    }
}
