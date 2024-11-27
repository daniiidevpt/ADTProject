using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ModularWeaponMorph : MonoBehaviour
{
    public List<WeaponPartMorph> weaponParts = new List<WeaponPartMorph>();
    private Mesh weaponMesh = null;

    private Vector3[] currentVertices = null;

    void Start()
    {
        weaponMesh = new Mesh { name = "Modular Weapon" };
        GetComponent<MeshFilter>().mesh = weaponMesh;

        AssembleWeaponFromParts();

        //Invoke("MorphToTarget", 3f);
    }

    private void AssembleWeaponFromParts()
    {
        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        List<Vector2> allUVs = new List<Vector2>();

        int vertexOffset = 0;

        foreach (var part in weaponParts)
        {
            if (part.sourcePart == null)
            {
                Debug.LogWarning($"Missing source part in WeaponPartMorph. Skipping this part.");
                continue;
            }

            allVertices.AddRange(part.sourcePart.vertices);
            allUVs.AddRange(part.sourcePart.uvs);

            foreach (int tri in part.sourcePart.triangles)
            {
                allTriangles.Add(tri + vertexOffset);
            }

            vertexOffset += part.sourcePart.vertices.Length;
        }

        weaponMesh.vertices = allVertices.ToArray();
        weaponMesh.triangles = allTriangles.ToArray();
        weaponMesh.uv = allUVs.ToArray();

        weaponMesh.RecalculateNormals();
        weaponMesh.RecalculateBounds();

        currentVertices = weaponMesh.vertices;
    }

    public void MorphToTarget()
    {
        StartCoroutine(MorphParts());
    }

    private IEnumerator MorphParts()
    {
        float time = 0f;

        List<Vector3[]> sourceVerticesList = new List<Vector3[]>();
        List<Vector3[]> targetVerticesList = new List<Vector3[]>();

        foreach (var part in weaponParts)
        {
            if (part.sourcePart == null)
            {
                Debug.LogWarning($"Missing source part in WeaponPartMorph. Skipping this part.");
                continue;
            }

            sourceVerticesList.Add(part.sourcePart.vertices);

            if (part.targetPart != null)
            {
                if (part.useDynamicMapping)
                {
                    targetVerticesList.Add(RemappedVertices(part.sourcePart.vertices, part.targetPart.vertices));
                }
                else if (part.sourcePart.vertices.Length == part.targetPart.vertices.Length)
                {
                    targetVerticesList.Add(part.targetPart.vertices);
                }
                else
                {
                    Debug.LogWarning($"Vertex count mismatch in {part.sourcePart.name}. Skipping morph for this part.");
                    targetVerticesList.Add(part.sourcePart.vertices);
                }
            }
            else
            {
                targetVerticesList.Add(part.sourcePart.vertices);
            }
        }

        float morphDuration = GetMaxMorphDuration();

        while (time < morphDuration)
        {
            float t = time / morphDuration;

            int vertexIndex = 0;

            for (int partIndex = 0; partIndex < weaponParts.Count; partIndex++)
            {
                var part = weaponParts[partIndex];
                if (part.sourcePart == null) continue;

                var sourceVertices = sourceVerticesList[partIndex];
                var targetVertices = targetVerticesList[partIndex];

                for (int i = 0; i < sourceVertices.Length; i++)
                {
                    currentVertices[vertexIndex] = Vector3.Lerp(sourceVertices[i], targetVertices[i], t);
                    vertexIndex++;
                }
            }

            weaponMesh.vertices = currentVertices;
            weaponMesh.RecalculateNormals();

            time += Time.deltaTime;
            yield return null;
        }

        int finalIndex = 0;
        for (int partIndex = 0; partIndex < weaponParts.Count; partIndex++)
        {
            var part = weaponParts[partIndex];
            if (part.sourcePart == null) continue;

            var targetVertices = targetVerticesList[partIndex];

            for (int i = 0; i < targetVertices.Length; i++)
            {
                currentVertices[finalIndex] = targetVertices[i];
                finalIndex++;
            }
        }

        weaponMesh.vertices = currentVertices;
        weaponMesh.RecalculateNormals();
    }

    private Vector3[] RemappedVertices(Vector3[] sourceVertices, Vector3[] targetVertices)
    {
        Vector3[] remapped = new Vector3[sourceVertices.Length];

        Bounds sourceBounds = GetBounds(sourceVertices);
        Bounds targetBounds = GetBounds(targetVertices);

        for (int i = 0; i < sourceVertices.Length; i++)
        {
            int closestIndex = -1;
            float closestDistance = float.MaxValue;

            Vector3 normalizedSource = NormalizeToBounds(sourceVertices[i], sourceBounds);

            for (int j = 0; j < targetVertices.Length; j++)
            {
                Vector3 normalizedTarget = NormalizeToBounds(targetVertices[j], targetBounds);

                float distance = Vector3.Distance(normalizedSource, normalizedTarget);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = j;
                }
            }

            remapped[i] = closestIndex != -1 ? targetVertices[closestIndex] : sourceVertices[i];
        }

        return remapped;
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

    private float GetMaxMorphDuration()
    {
        float maxDuration = 0f;
        foreach (var part in weaponParts)
        {
            if (part.sourcePart != null && part.targetPart != null)
            {
                maxDuration = Mathf.Max(maxDuration, part.morphDuration);
            }
        }
        return maxDuration > 0 ? maxDuration : 1f;
    }

    private Vector3 GetMeshPivot(Vector3[] vertices)
    {
        if (vertices == null || vertices.Length == 0)
            return Vector3.zero; // Default pivot if no vertices exist

        // Initialize pivot to the first vertex
        Vector3 pivot = vertices[0];
        float minDistance = pivot.magnitude;

        // Iterate over vertices to find the closest to the origin
        foreach (Vector3 vertex in vertices)
        {
            float distance = vertex.magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                pivot = vertex;
            }
        }

        return pivot;
    }

    #region DEBUG

    private void OnDrawGizmosSelected()
    {
        // Visualize Vertex Mappings

        if (weaponParts == null || weaponParts.Count == 0) return;

        foreach (var part in weaponParts)
        {
            if (part.sourcePart == null || part.targetPart == null || !part.showVertexMapping || !part.showDebug) continue;

            // Get source and target vertices
            Vector3[] sourceVertices = part.sourcePart.vertices;
            Vector3[] targetVertices = part.useDynamicMapping ?
                RemappedVertices(part.sourcePart.vertices, part.targetPart.vertices) 
                : 
                part.targetPart.vertices; 
            ;

            // Draw lines and spheres
            for (int i = 0; i < sourceVertices.Length; i++)
            {
                Vector3 sourceWorldPosition = transform.TransformPoint(sourceVertices[i]);
                Vector3 targetWorldPosition = transform.TransformPoint(targetVertices[i]);

                // Draw the start sphere (red)
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(sourceWorldPosition, 0.005f);

                // Draw the end sphere (green)
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(targetWorldPosition, 0.005f);

                // Simulate gradient line from red to green
                int gradientSteps = 3; // Number of segments in the gradient
                for (int step = 0; step < gradientSteps; step++)
                {
                    float tStart = step / (float)gradientSteps;
                    float tEnd = (step + 1) / (float)gradientSteps;

                    Vector3 gradientStart = Vector3.Lerp(sourceWorldPosition, targetWorldPosition, tStart);
                    Vector3 gradientEnd = Vector3.Lerp(sourceWorldPosition, targetWorldPosition, tEnd);

                    Gizmos.color = Color.Lerp(Color.red, Color.green, tStart);
                    Gizmos.DrawLine(gradientStart, gradientEnd);
                }
            }
        }

        // Draw Bounding Boxes

        if (weaponParts == null || weaponParts.Count == 0) return;

        foreach (var part in weaponParts)
        {
            if (part.sourcePart == null || part.targetPart == null || !part.showBoundingBoxes || !part.showDebug) continue;

            // Draw bounding box for the source part
            Bounds sourceBounds = GetBounds(part.sourcePart.vertices);
            DrawBounds(sourceBounds, Color.blue, $"{part.sourcePart.name}: Source Bounds");

            // Draw bounding box for the target part
            Bounds targetBounds = GetBounds(part.targetPart.vertices);
            DrawBounds(targetBounds, Color.green, $"{part.targetPart.name}: Target Bounds");
        }

        // Draw Pivot Points

        if (weaponParts == null || weaponParts.Count == 0) return;

        foreach (var part in weaponParts)
        {
            if (part.sourcePart == null || part.targetPart == null || !part.showPivotPoints || !part.showDebug) continue;

            Vector3 sourcePivot = GetMeshPivot(part.sourcePart.vertices);
            Vector3 targetPivot = GetMeshPivot(part.targetPart.vertices);

            // Draw pivot for source part
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.TransformPoint(sourcePivot), 0.05f);

            // Draw pivot for target part
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(targetPivot + part.targetPart.offset), 0.05f);
        }
    }

    private void DrawBounds(Bounds bounds, Color color, string label)
    {
        Gizmos.color = color;

        // Draw the bounding box
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        // Draw label at the center of the bounding box
        Gizmos.color = Color.white;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        UnityEditor.Handles.Label(bounds.center, label, style);
    }

    #endregion
}
