using UnityEngine;
using Zenject;

[RequireComponent(typeof(PipeTemplateView))]
public class PipeTemplatePresenter : Presenter
{
    [SerializeField] private Fuel _fuelType;

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
    }

    private void OnDisable()
    {
        Model.PlacedOnGrid -= View.PlaySoundOnPlaced;
    }

    [Inject]
    private void Construct(IGrid grid)
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

        grid.Place(Model);
    }
}
