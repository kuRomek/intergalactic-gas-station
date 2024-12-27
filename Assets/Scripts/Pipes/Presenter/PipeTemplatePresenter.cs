using System.Collections;
using UnityEngine;
using Zenject;
using IntergalacticGasStation.Fuel;
using IntergalacticGasStation.LevelGrid;
using IntergalacticGasStation.StructureElements;

namespace IntergalacticGasStation
{
    namespace Pipes
    {
        [RequireComponent(typeof(PipeTemplateView))]
        public class PipeTemplatePresenter : Presenter
        {
            [SerializeField] private FuelType _fuelType;

            [Inject] private IGrid _grid;

            public new PipeTemplate Model => base.Model as PipeTemplate;

            public new PipeTemplateView View => base.View as PipeTemplateView;

            private void OnValidate()
            {
                GetComponent<PipeTemplateView>().SetColor(_fuelType);
            }

            private void Awake()
            {
                View.SetColor(_fuelType);
            }

            private void OnEnable()
            {
                Model.Enable();
                Model.PlacedOnGrid += View.PlaySoundOnPlaced;
                Model.Providing += View.SetOutline;
                Model.ProvidingStopped += View.RemoveOutline;

                StartCoroutine(WaitForOtherTemplatesToPlace());
            }

            private void OnDisable()
            {
                Model.Disable();
                Model.PlacedOnGrid -= View.PlaySoundOnPlaced;
                Model.Providing -= View.SetOutline;
                Model.ProvidingStopped -= View.RemoveOutline;

                _grid.Remove(Model);
            }

            private IEnumerator WaitForOtherTemplatesToPlace()
            {
                yield return new WaitForEndOfFrame();
                _grid.Place(Model);
            }

            [Inject]
            private void Construct()
            {
                PipePiecePresenter[] pipePiecePresenters = GetComponentsInChildren<PipePiecePresenter>();

                PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

                for (int i = 0; i < pipePieces.Length; i++)
                {
                    PipePiece pipePiece = new PipePiece(
                        pipePiecePresenters[i].transform.position,
                        pipePiecePresenters[pipePiecePresenters.Length / 2].transform.position,
                        pipePiecePresenters[i].transform.rotation,
                        _fuelType);

                    pipePiecePresenters[i].Init(pipePiece);
                    pipePieces[i] = pipePiecePresenters[i].Model;
                }

                Init(new PipeTemplate(pipePieces, _fuelType));
            }
        }
    }
}
