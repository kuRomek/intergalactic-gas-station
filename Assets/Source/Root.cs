using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private PipeTemplatePresenter[] _pipeTemplatePresenters;
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private GridPresenter _gridPresenter;
    [SerializeField] private PipeDraggerPresenter _pipeDraggerPresenter;
    [SerializeField] private StationPresenter _stationPresenter;
    [SerializeField] private PresenterFactory _presenterFactory;
    [SerializeField] private ShipTrajectory _leftTrajectory;
    [SerializeField] private ShipTrajectory _rightTrajectory;
    [SerializeField] private ShipTrajectory _topTrajectory;

    private void Awake()
    {
        Station station = new Station(_leftTrajectory.RefuelingPoint.position, _rightTrajectory.RefuelingPoint.position, _topTrajectory.RefuelingPoint.position);
        _stationPresenter.Init(station);

        Grid grid = new Grid(_gridPresenter.transform.position, _gridPresenter.transform.rotation);
        _gridPresenter.Init(grid);

        foreach (PipeTemplatePresenter pipeTemplatePresenter in _pipeTemplatePresenters)
        {
            PipePiecePresenter[] pipePiecePresenters = pipeTemplatePresenter.GetComponentsInChildren<PipePiecePresenter>();

            PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

            for (int i = 0; i < pipePieces.Length; i++)
            {
                pipePiecePresenters[i].Init(new PipePiece(grid.CalculateGridPosition(pipePiecePresenters[i].transform.position)));
                pipePieces[i] = pipePiecePresenters[i].Model;
            }

            pipeTemplatePresenter.Init(new PipeTemplate(pipePieces));

            grid.Place(pipeTemplatePresenter.Model);
        }

        PipeDragger pipeDragger = new PipeDragger(_inputController, grid);
        _pipeDraggerPresenter.Init(pipeDragger);

        Ship ship = new Ship(_leftTrajectory, new Fuel[] { Fuel.Red });
        _presenterFactory.CreateShip(ship);
        _stationPresenter.Model.Arrive(ship);
    }
}
