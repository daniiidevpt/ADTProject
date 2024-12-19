using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using Unity.Collections;

[ExecuteInEditMode]
public class MeshAdjuster : MonoBehaviour
{
    public WeaponPart sourcePart; // Source mesh input
    public WeaponPart targetPart; // Target mesh input

    [Header("Settings")]
    public bool adjustToTargetVertexCount = true;
    public bool enableGizmos = true;
    public float visualizationStepDuration = 2f; // Time between steps

    [Header("Debug Info")]
    [ReadOnly] public int sourceVertexCount;
    [ReadOnly] public int targetVertexCount;
    [ReadOnly] public int adjustedVertexCount;

    private Vector3[] adjustedVertices;
    private bool adjustmentDone = false;
    private bool showAdjustedMesh = false; // Controls step-by-step visualization

    private void Update()
    {
        if (sourcePart != null)
        {
            sourceVertexCount = sourcePart.vertices.Length;
        }

        if (targetPart != null)
        {
            targetVertexCount = targetPart.vertices.Length;
        }
    }

    public void AdjustMesh()
    {
        if (sourcePart == null || targetPart == null)
        {
            Debug.LogWarning("Source or Target Part is missing.");
            return;
        }

        sourceVertexCount = sourcePart.vertices.Length;
        targetVertexCount = targetPart.vertices.Length;

        if (adjustToTargetVertexCount)
        {
            adjustedVertices = AdjustVertices(sourcePart.vertices, targetVertexCount);
            adjustedVertexCount = adjustedVertices.Length;
        }

        adjustmentDone = true;
        showAdjustedMesh = false;

        // Start visualization coroutine
        StartCoroutine(ShowSteps());
    }

    private IEnumerator ShowSteps()
    {
        // Step 1: Show Source Mesh
        showAdjustedMesh = false;
        yield return new WaitForSeconds(visualizationStepDuration);

        // Step 2: Show Adjusted Mesh
        showAdjustedMesh = true;
        yield return new WaitForSeconds(visualizationStepDuration);
    }

    private Vector3[] AdjustVertices(Vector3[] sourceVertices, int targetCount)
    {
        if (sourceVertices.Length > targetCount)
        {
            return DecimateVertices(sourceVertices, targetCount);
        }
        else if (sourceVertices.Length < targetCount)
        {
            return AddVertices(sourceVertices, targetCount);
        }
        return sourceVertices;
    }

    private Vector3[] DecimateVertices(Vector3[] vertices, int targetCount)
    {
        if (vertices.Length <= targetCount)
            return vertices; // No decimation needed

        // Step 1: Calculate grid cell size based on target count
        Bounds bounds = GetBounds(vertices);
        float gridSize = Mathf.Pow(bounds.size.magnitude, 1f / 3f) / Mathf.Pow(targetCount, 1f / 3f);

        // Step 2: Group vertices into cells
        Dictionary<Vector3Int, List<Vector3>> grid = new Dictionary<Vector3Int, List<Vector3>>();
        foreach (Vector3 vertex in vertices)
        {
            Vector3 normalized = NormalizeToBounds(vertex, bounds);
            Vector3Int cell = new Vector3Int(
                Mathf.FloorToInt(normalized.x / gridSize),
                Mathf.FloorToInt(normalized.y / gridSize),
                Mathf.FloorToInt(normalized.z / gridSize)
            );

            if (!grid.ContainsKey(cell))
                grid[cell] = new List<Vector3>();

            grid[cell].Add(vertex);
        }

        // Step 3: Average vertices in each cell
        List<Vector3> decimatedVertices = new List<Vector3>();
        foreach (var cell in grid)
        {
            Vector3 average = Vector3.zero;
            foreach (var vertex in cell.Value)
            {
                average += vertex;
            }
            average /= cell.Value.Count;
            decimatedVertices.Add(average);
        }

        // Step 4: Ensure the result matches the target count by trimming or duplicating
        while (decimatedVertices.Count > targetCount)
        {
            decimatedVertices.RemoveAt(decimatedVertices.Count - 1);
        }

        while (decimatedVertices.Count < targetCount)
        {
            decimatedVertices.Add(decimatedVertices[decimatedVertices.Count - 1]);
        }

        return decimatedVertices.ToArray();
    }


    private Vector3[] AddVertices(Vector3[] vertices, int targetCount)
    {
        List<Vector3> expandedVertices = new List<Vector3>(vertices);
        while (expandedVertices.Count < targetCount)
        {
            int edgeIndex = Random.Range(0, expandedVertices.Count - 1);
            Vector3 midpoint = (expandedVertices[edgeIndex] + expandedVertices[edgeIndex + 1]) / 2f;
            expandedVertices.Insert(edgeIndex + 1, midpoint);
        }
        return expandedVertices.ToArray();
    }

    private Bounds GetBounds(Vector3[] vertices)
    {
        Bounds bounds = new Bounds(vertices[0], Vector3.zero);
        foreach (Vector3 vertex in vertices)
        {
            bounds.Encapsulate(vertex);
        }
        return bounds;
    }

    private Vector3 NormalizeToBounds(Vector3 point, Bounds bounds)
    {
        return new Vector3(
            (point.x - bounds.min.x) / bounds.size.x,
            (point.y - bounds.min.y) / bounds.size.y,
            (point.z - bounds.min.z) / bounds.size.z
        );
    }

    private void OnDrawGizmos()
    {
        if (!enableGizmos || !adjustmentDone) return;

        // Draw Source Mesh (Step 1)
        if (!showAdjustedMesh && sourcePart != null)
        {
            DrawMeshGizmos(sourcePart.vertices, sourcePart.triangles, Color.red);
        }

        // Draw Adjusted Mesh (Step 2)
        if (showAdjustedMesh && adjustedVertices != null)
        {
            // Create temporary triangles for visualization if needed
            DrawMeshGizmos(adjustedVertices, sourcePart.triangles, Color.green);
        }
    }

    private void DrawMeshGizmos(Vector3[] vertices, int[] triangles, Color color)
    {
        Gizmos.color = color;

        // Draw vertices
        foreach (Vector3 vertex in vertices)
        {
            Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.01f);
        }

        // Draw wireframe
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            Gizmos.DrawLine(v0, v1);
            Gizmos.DrawLine(v1, v2);
            Gizmos.DrawLine(v2, v0);
        }
    }
}
