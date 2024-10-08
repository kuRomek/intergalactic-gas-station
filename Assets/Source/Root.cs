using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private PipeTemplatePresenter[] _pipeTemplatePresenters;
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private GridPresenter _gridPresenter;
    [SerializeField] private PipeDraggerPresenter _pipeDraggerPresenter;
    [SerializeField] private StationPresenter _stationPresenter;
    [SerializeField] private PresenterFactory _presenterFactory;
    [SerializeField] private Transform[] _shipSpawningAreas;
    [SerializeField] private Transform[] _refuelingPoints;
    [SerializeField] private Transform _tanksPlace;
    [SerializeField] private Transform _shipsWaitingPlace;
    [SerializeField] private LevelSetup _levelSetup;
    [SerializeField] private UIMenu _levelCompleteMenu;
    
    private Grid _grid;
    private LevelState _levelState;

    private void Awake()
    {
        Grid grid = new Grid(_gridPresenter.transform.position, _gridPresenter.transform.rotation);
        _gridPresenter.Init(grid);
        _grid = grid;

        TankContainer tankContainer = new TankContainer(_tanksPlace.position);

        foreach (TankCell tank in _levelSetup.Tanks)
            _presenterFactory.CreateTank(tankContainer.Add(tank.Type, tank.FuelType));

        Station station = new Station(_refuelingPoints, _shipSpawningAreas.Select(area => area.position).ToArray(), _gridPresenter.Model, tankContainer);
        _stationPresenter.Init(station);

        foreach (PipeTemplatePresenter pipeTemplatePresenter in _pipeTemplatePresenters)
        {
            pipeTemplatePresenter.enabled = true;
            pipeTemplatePresenter.Inited += PlaceOnGrid;
        }

        PipeDragger pipeDragger = new PipeDragger(_inputController, grid);
        _pipeDraggerPresenter.Init(pipeDragger);

        Queue<Ship> shipsQueue = new Queue<Ship>();

        for (int i = 0; i < _levelSetup.ShipCount; i++)
        {
            Ship newShip = new Ship(_shipsWaitingPlace, new Fuel[] { Fuel.Default });
            shipsQueue.Enqueue(newShip);
            _presenterFactory.CreateShip(newShip);
        }

        (_levelState = new LevelState(_levelCompleteMenu, shipsQueue, station)).Enable();
    }

    private void OnDisable()
    {
        _levelState.Disable();
    }

    private void PlaceOnGrid(PipeTemplate pipeTemplate)
    {
        _grid.Place(pipeTemplate);
    }
}
