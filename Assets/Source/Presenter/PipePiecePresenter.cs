using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PipePiecePresenter : Presenter
{
    private PipeShapes _pipeShapes;
    private MeshFilter _meshFilter;

    public new PipePiece Model => base.Model as PipePiece;

    private void Awake()
    {
        _pipeShapes = (PipeShapes)Resources.Load("Pipe Shapes");
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        Model.ConnectionIsEstablished += ChangeView;
    }

    private void OnDisable()
    {
        Model.ConnectionIsEstablished -= ChangeView;
    }

    private void ChangeView(bool[] connections)
    {
        Mesh mesh = _pipeShapes.GetShape(connections, out float rotation);

        _meshFilter.mesh = mesh;
        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
    }
}
