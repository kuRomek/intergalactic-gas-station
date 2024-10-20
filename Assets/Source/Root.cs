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
    [SerializeField] private UIMenu _levelCompleteWindow;
    [SerializeField] private UIMenu _loseWindow;
    [SerializeField] private TimerView _timerView;
    [SerializeField] private TankContainerShifter _tankContainerShifter;
    
    private Grid _grid;
    private LevelState _levelState;

    private void Awake()
    {
        Grid grid = new Grid(_gridPresenter.transform.position, _gridPresenter.transform.rotation);
        _gridPresenter.Init(grid);
        _grid = grid;

        foreach (PipeTemplatePresenter pipeTemplatePresenter in _pipeTemplatePresenters)
        {
            PipePiecePresenter[] pipePiecePresenters = pipeTemplatePresenter.GetComponentsInChildren<PipePiecePresenter>();

            PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

            for (int i = 0; i < pipePieces.Length; i++)
            {
                PipePiece pipePiece = new PipePiece(pipePiecePresenters[i].transform.position, pipeTemplatePresenter.transform.position, pipeTemplatePresenter.FuelType);
                pipePiecePresenters[i].Init(pipePiece);
                pipePieces[i] = pipePiecePresenters[i].Model;
            }

            pipeTemplatePresenter.Init(new PipeTemplate(pipePieces, pipeTemplatePresenter.FuelType));
            pipeTemplatePresenter.View.Init(pipeTemplatePresenter.FuelType);

            _grid.Place(pipeTemplatePresenter.Model);
        }

        TankContainer tankContainer = new TankContainer(_tanksPlace.position, _presenterFactory);
        _tankContainerShifter.Init(tankContainer);

        Station station = new Station(_refuelingPoints, _shipSpawningAreas.Select(area => area.position).ToArray(), grid, tankContainer);
        _stationPresenter.Init(station);

        if (_levelSetup != null)
        {
            foreach (TankSetup tank in _levelSetup.Tanks)
                tankContainer.Add(tank.Size, tank.FuelType);

            List<Ship> shipsQueue = new List<Ship>();

            for (int i = 0; i < _levelSetup.Ships.Length; i++)
            {
                Ship newShip = new Ship(_shipsWaitingPlace.position, _levelSetup.Ships[i]);
                shipsQueue.Add(newShip);
                _presenterFactory.CreateShip(newShip);
            }

            Utils.Shuffle(_levelSetup.Tanks);
            Utils.Shuffle(_levelSetup.Ships);

            Timer timer = new Timer(_levelSetup.TimeInSeconds);
            _timerView.Init(timer);

            _levelState = new LevelState(_levelCompleteWindow, _loseWindow, tankContainer, shipsQueue, station, timer);
        }
        else
        {
            Timer timer = new Timer(120);
            _timerView.Init(timer);

            _levelState = new LevelState(_levelCompleteWindow, _loseWindow, tankContainer, _shipsWaitingPlace.position, _presenterFactory, station, timer);
        }

        _levelState.Enable();

        _inputController.Init(_levelState);
        _inputController.enabled = true;

        PipeDragger pipeDragger = new PipeDragger(_inputController, grid);
        _pipeDraggerPresenter.Init(pipeDragger);
    }

    private void OnDisable()
    {
        _levelState.Disable();
    }
}
