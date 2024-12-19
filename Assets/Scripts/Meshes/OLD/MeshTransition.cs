using UnityEngine;

[CreateAssetMenu(fileName = "NewMeshTransition", menuName = "MeshTransition", order = 1)]
public class MeshTransition : ScriptableObject
{
    public Mesh startMesh;
    public Mesh intermediateMesh;
    public Mesh finalMesh;
}

