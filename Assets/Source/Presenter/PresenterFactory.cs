using UnityEngine;

public class PresenterFactory : MonoBehaviour
{
    [SerializeField] private ShipPresenter _shipPresenter;
    [SerializeField] private TankPresenter _tankPresenter;

    public void CreateShip(Ship shipModel)
    {
        CreatePresenter(_shipPresenter, shipModel);
    }

    public void CreateTank(Tank tankModel)
    {
        TankPresenter presenter = CreatePresenter(_tankPresenter, tankModel) as TankPresenter;
        presenter.View.Init(tankModel.FuelType);
    }

    public Presenter CreatePresenter(Presenter presenterTemplate, Transformable model)
    {
        Presenter presenter = Instantiate(presenterTemplate);
        presenter.Init(model);
        return presenter;
    }
}
