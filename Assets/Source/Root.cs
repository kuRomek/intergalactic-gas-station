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
        Grid grid = new Grid(_gridPresenter.transform.position, _gridPresenter.transform.rotation);
        _gridPresenter.Init(grid);

        Station station = new Station(_leftTrajectory.RefuelingPoint.position, _rightTrajectory.RefuelingPoint.position, _topTrajectory.RefuelingPoint.position, grid);
        _stationPresenter.Init(station);

        foreach (PipeTemplatePresenter pipeTemplatePresenter in _pipeTemplatePresenters)
        {
            PipePiecePresenter[] pipePiecePresenters = pipeTemplatePresenter.GetComponentsInChildren<PipePiecePresenter>();

            PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

            for (int i = 0; i < pipePieces.Length; i++)
            {
                pipePiecePresenters[i].Init(new PipePiece(pipePiecePresenters[i].transform.position, pipeTemplatePresenter.transform.position));
                pipePieces[i] = pipePiecePresenters[i].Model;
            }

            pipeTemplatePresenter.Init(new PipeTemplate(pipePieces));

            grid.Place(pipeTemplatePresenter.Model);
        }

        PipeDragger pipeDragger = new PipeDragger(_inputController, grid);
        _pipeDraggerPresenter.Init(pipeDragger);

        Ship ship1 = new Ship(_leftTrajectory, new Fuel[] { Fuel.Default });
        _presenterFactory.CreateShip(ship1);
        _stationPresenter.Model.Arrive(ship1);

        Ship ship2 = new Ship(_topTrajectory, new Fuel[] { Fuel.Default });
        _presenterFactory.CreateShip(ship2);
        _stationPresenter.Model.Arrive(ship2);

        Ship ship3 = new Ship(_rightTrajectory, new Fuel[] { Fuel.Default });
        _presenterFactory.CreateShip(ship3);
        _stationPresenter.Model.Arrive(ship3);
    }
}
