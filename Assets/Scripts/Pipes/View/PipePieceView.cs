using UnityEngine;
using Zenject;
using StructureElements;

namespace Pipes
{
    [RequireComponent(typeof(MeshFilter))]
    public class PipePieceView : View
    {
        private const string PipeShapes = "Pipe Shapes";

        private PipeShapes _pipeShapes;
        private MeshFilter _meshFilter;
        private Mesh _originalMesh;
        private Quaternion _originalRotation;

        public void ChangeToOriginalView()
        {
            _meshFilter.mesh = _originalMesh;
            transform.rotation = _originalRotation;
        }

        public void ChangeShape(bool[] connections)
        {
            Mesh mesh = _pipeShapes.GetShape(connections, out float rotation);

            _meshFilter.mesh = mesh;
            transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
        }

        [Inject]
        private void Construct()
        {
            _pipeShapes = (PipeShapes)Resources.Load(PipeShapes);
            _meshFilter = GetComponent<MeshFilter>();

            _originalMesh = _meshFilter.mesh;
            _originalRotation = transform.rotation;
        }
    }
}
