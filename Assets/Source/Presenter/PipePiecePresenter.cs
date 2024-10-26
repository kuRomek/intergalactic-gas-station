using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PipePiecePresenter : Presenter
{
    private PipeShapes _pipeShapes;
    private MeshFilter _meshFilter;
    private Mesh _originalMesh;
    private Quaternion _originalRotation;

    public new PipePiece Model => base.Model as PipePiece;

    private void Awake()
    {
        _pipeShapes = (PipeShapes)Resources.Load("Pipe Shapes");
        _meshFilter = GetComponent<MeshFilter>();

        _originalMesh = _meshFilter.mesh;
        _originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        Model.ConnectionIsEstablishing += ChangeView;
        Model.OriginalViewRecovering += RecoverOriginalView;
    }

    private void OnDisable()
    {
        Model.ConnectionIsEstablishing -= ChangeView;
        Model.OriginalViewRecovering -= RecoverOriginalView;
    }

    private void RecoverOriginalView()
    {
        _meshFilter.mesh = _originalMesh;
        transform.rotation = _originalRotation;
    }

    private void ChangeView(bool[] connections)
    {
        Mesh mesh = _pipeShapes.GetShape(connections, out float rotation);

        _meshFilter.mesh = mesh;
        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
    }
}
