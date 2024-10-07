using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Root : MonoBehaviour
{
    [SerializeField] private PipeTemplatePresenter[] _pipeTemplatePresenters;
    [SerializeField] private TankPresenter[] _tankPresenters;
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private GridPresenter _gridPresenter;
    [SerializeField] private PipeDraggerPresenter _pipeDraggerPresenter;
    [SerializeField] private StationPresenter _stationPresenter;
    [SerializeField] private PresenterFactory _presenterFactory;
    [SerializeField] private Transform[] _shipSpawningAreas;
    [SerializeField] private Transform[] _refuelingPoints;
    [SerializeField] private Transform _tanksPlace;
    [SerializeField] private Transform _shipsQueue;
    
    private Queue<Ship> _shipsInQueue = new Queue<Ship>();

    private void Awake()
    {
        Grid grid = new Grid(_gridPresenter.transform.position, _gridPresenter.transform.rotation);
        _gridPresenter.Init(grid);

        TankContainer tankContainer = new TankContainer(_tanksPlace.position);
        _presenterFactory.CreateTank(tankContainer.Add(Tank.Type.Big, Fuel.Default));
        _presenterFactory.CreateTank(tankContainer.Add(Tank.Type.Small, Fuel.Default));
        _presenterFactory.CreateTank(tankContainer.Add(Tank.Type.Medium, Fuel.Default));

        Station station = new Station(_refuelingPoints, _shipSpawningAreas.Select(area => area.position).ToArray(), _gridPresenter.Model, tankContainer);
        _stationPresenter.Init(station);

        station.PlaceFreed += LetShipOnStation;

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

        for (int i = 0; i < 10; i++)
        {
            Ship newShip = new Ship(_shipsQueue, new Fuel[] { Fuel.Default });
            _shipsInQueue.Enqueue(newShip);
            _presenterFactory.CreateShip(newShip);
        }

        station.Arrive(_shipsInQueue.Dequeue());
        station.Arrive(_shipsInQueue.Dequeue());
        station.Arrive(_shipsInQueue.Dequeue());
    }

    private void OnDisable()
    {
        _stationPresenter.Model.PlaceFreed -= LetShipOnStation;
    }

    private void LetShipOnStation()
    {
        if (_shipsInQueue.Count > 0)
            _stationPresenter.Model.Arrive(_shipsInQueue.Dequeue());
    }
}
