using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private StationPresenter _stationPresenter;
    [SerializeField] private PresenterFactory _presenterFactory;
    [SerializeField] private ShipTrajectory _leftTrajectory;
    [SerializeField] private ShipTrajectory _rightTrajectory;
    [SerializeField] private ShipTrajectory _topTrajectory;

    private void Awake()
    {
        Station station = new Station(_leftTrajectory.RefuelingPoint.position, _rightTrajectory.RefuelingPoint.position, _topTrajectory.RefuelingPoint.position);
        _stationPresenter.Init(station);

        Ship ship = new Ship(_leftTrajectory, new Fuel[] { Fuel.Red });
        _presenterFactory.CreateShip(ship);
        _stationPresenter.Model.Arrive(ship);
    }
}
