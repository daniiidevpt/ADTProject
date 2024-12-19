using System.Collections;
using UnityEngine;

public class MeshMorpher : MonoBehaviour
{
    public MeshTransition meshTransition; // Assign the ScriptableObject here
    public float transitionDuration = 2f; // Total time for the morph
    private MeshFilter meshFilter;

    public Mesh test1;
    public Mesh test2;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshTransition == null)
        {
            Debug.LogError("MeshFilter or MeshTransition is missing.");
            return;
        }

        // Start morphing process
        //StartCoroutine(MorphMesh());
        StartCoroutine(MorphBetweenMeshes(test1, test2, 5f));
    }

    private IEnumerator MorphMesh()
    {
        Mesh startMesh = meshTransition.startMesh;
        Mesh intermediateMesh = meshTransition.intermediateMesh;
        Mesh finalMesh = meshTransition.finalMesh;

        // Morph from Start to Intermediate
        yield return MorphBetweenMeshes(startMesh, intermediateMesh, transitionDuration / 2);

        // Morph from Intermediate to Final
        yield return MorphBetweenMeshes(intermediateMesh, finalMesh, transitionDuration / 2);
    }

    public IEnumerator MorphBetweenMeshes(Mesh fromMesh, Mesh toMesh, float duration)
    {
        Vector3[] fromVertices = fromMesh.vertices;
        Vector3[] toVertices = toMesh.vertices;
        Vector3[] currentVertices = new Vector3[fromVertices.Length];

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < fromVertices.Length; i++)
            {
                currentVertices[i] = Vector3.Lerp(fromVertices[i], toVertices[i], t);
            }

            Mesh newMesh = new Mesh
            {
                vertices = currentVertices,
                triangles = fromMesh.triangles,
                uv = fromMesh.uv,
                normals = fromMesh.normals
            };
            meshFilter.mesh = newMesh;

            yield return null;
        }

        // Ensure final state
        meshFilter.mesh = toMesh;
    }
}
