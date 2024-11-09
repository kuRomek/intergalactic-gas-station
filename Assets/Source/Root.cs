using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(PresenterFactory))]
public class Root : MonoBehaviour
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private Transform _gridTransform;
    [SerializeField] private Transform[] _shipSpawningAreas;
    [SerializeField] private Transform[] _refuelingPoints;
    [SerializeField] private Transform _tanksPlace;
    [SerializeField] private Transform _shipsWaitingPlace;
    [SerializeField] private LevelSetup _levelSetup;
    [SerializeField] private UIMenu _levelCompleteWindow;
    [SerializeField] private UIMenu _loseWindow;
    [SerializeField] private UIMenu _pauseWindow;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TimerView _timerView;
    [SerializeField] private ShipCounter _shipCounter;
    [SerializeField] private TankContainerShifter _tankContainerShifter;
    [SerializeField] private GridChanger _gridChanger;
    
    private PresenterFactory _presenterFactory;
    private LevelState _levelState;
    private PipeDragger _pipeDragger;
    private Station _station;
    private Grid _grid;

    private void Awake()
    {
        _presenterFactory = GetComponent<PresenterFactory>();

        TankContainer tankContainer = new TankContainer(_tanksPlace.position, _presenterFactory);
        _tankContainerShifter.Init(tankContainer);

        _station = new Station(_refuelingPoints, _shipSpawningAreas.Select(area => area.position).ToArray(), _grid, tankContainer);

        if (_levelSetup != null)
        {
            Utils.Shuffle(_levelSetup.Tanks);
            Utils.Shuffle(_levelSetup.Ships);

            foreach (TankSetup tank in _levelSetup.Tanks)
                tankContainer.Add(tank.Size, tank.FuelType);

            List<Ship> shipsQueue = new List<Ship>();

            for (int i = 0; i < _levelSetup.Ships.Length; i++)
            {
                Ship newShip = new Ship(_shipsWaitingPlace.position, _levelSetup.Ships[i]);
                shipsQueue.Add(newShip);
                _presenterFactory.CreateShip(newShip);
            }

            Timer timer = new Timer(_levelSetup.TimeInSeconds);
            _timerView.Init(timer);

            _levelState = new NonInfiniteLevelState(
                _levelCompleteWindow, 
                _loseWindow, 
                _pauseWindow, 
                _pauseButton, 
                _station, 
                shipsQueue, 
                timer);
        }
        else
        {
            Timer timer = new Timer(120);
            _timerView.Init(timer);

            _levelState = new InfiniteLevelState(
                _levelCompleteWindow, 
                _loseWindow, 
                _pauseWindow, 
                _pauseButton, 
                tankContainer, 
                _shipsWaitingPlace.position, 
                _presenterFactory, 
                _gridChanger, 
                _station, 
                timer);
        }

        _shipCounter.Init(_levelState);

        _inputController.Init(_levelState);
        _inputController.enabled = true;

        _pipeDragger = new PipeDragger(_inputController, _grid);
    }

    private void OnEnable()
    {
        _station.Enable();
        _levelState.Enable();
        _pipeDragger.Enable();
    }

    private void OnDisable()
    {
        _station.Disable();
        _levelState.Disable();
        _pipeDragger.Disable();
    }

    private void Update()
    {
        _levelState.Update(Time.deltaTime);
    }

    [Inject]
    private void Construct(Grid grid)
    {
        _grid = grid;
    }
}
