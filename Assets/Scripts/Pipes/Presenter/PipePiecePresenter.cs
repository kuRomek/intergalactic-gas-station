using UnityEngine;
using IntergalacticGasStation.StructureElements;

namespace IntergalacticGasStation
{
    namespace Pipes
    {
        [RequireComponent(typeof(PipePieceView))]
        public class PipePiecePresenter : Presenter, IActivatable
        {
            public new PipePiece Model => base.Model as PipePiece;

            public new PipePieceView View => base.View as PipePieceView;

            public void Enable()
            {
                Model.ConnectionIsEstablishing += View.ChangeShape;
                Model.RemovedFromGrid += View.ChangeToOriginalView;
            }

            public void Disable()
            {
                Model.ConnectionIsEstablishing -= View.ChangeShape;
                Model.RemovedFromGrid -= View.ChangeToOriginalView;
            }
        }
    }
}
