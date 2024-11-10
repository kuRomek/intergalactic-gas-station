using UnityEngine;
using Zenject;

[RequireComponent(typeof(PipeTemplateView))]
public class PipeTemplatePresenter : Presenter
{
    [SerializeField] private Fuel _fuelType;

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
        Model.PlacedOnGrid += View.PlaySoundOnPlaced;
        Model.ProvidingFuel += View.SetOutline;
        Model.ProvidingStopped += View.RemoveOutline;

        _grid.Place(Model);
    }

    private void OnDisable()
    {
        _grid.Remove(Model);

        Model.PlacedOnGrid -= View.PlaySoundOnPlaced;
        Model.ProvidingFuel -= View.SetOutline;
        Model.ProvidingStopped -= View.RemoveOutline;
    }

    [Inject]
    private void Construct()
    {
        PipePiecePresenter[] pipePiecePresenters = GetComponentsInChildren<PipePiecePresenter>();

        PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

        for (int i = 0; i < pipePieces.Length; i++)
        {
            PipePiece pipePiece = new PipePiece(pipePiecePresenters[i].transform.position,
                pipePiecePresenters[pipePiecePresenters.Length / 2].transform.position,
                pipePiecePresenters[i].transform.rotation, _fuelType);

            pipePiecePresenters[i].Init(pipePiece);
            pipePieces[i] = pipePiecePresenters[i].Model;
        }

        Init(new PipeTemplate(pipePieces, _fuelType));
    }
}
