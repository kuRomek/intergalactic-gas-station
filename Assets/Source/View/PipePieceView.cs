using UnityEngine;
using Zenject;

[RequireComponent(typeof(MeshFilter))]
public class PipePieceView : View
{
    private PipeShapes _pipeShapes;
    private MeshFilter _meshFilter;
    private Mesh _originalMesh;
    private Quaternion _originalRotation;

    [Inject] 
    private void Construct()
    {
        _pipeShapes = (PipeShapes)Resources.Load("Pipe Shapes");
        _meshFilter = GetComponent<MeshFilter>();

        _originalMesh = _meshFilter.mesh;
        _originalRotation = transform.rotation;
    }

    public void ChengeToOriginalView()
    {
        _meshFilter.mesh = _originalMesh;
        transform.rotation = _originalRotation;
    }

    public void ChangeView(bool[] connections)
    {
        Mesh mesh = _pipeShapes.GetShape(connections, out float rotation);

        _meshFilter.mesh = mesh;
        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
    }
}