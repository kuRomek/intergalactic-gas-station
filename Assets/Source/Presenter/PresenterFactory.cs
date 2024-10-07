using UnityEngine;

public class PresenterFactory : MonoBehaviour
{
    [SerializeField] private ShipPresenter _shipPresenter;
    [SerializeField] private TankPresenter _tankPresenter;

    public void CreateShip(Transformable shipModel)
    {
        CreatePresenter(_shipPresenter, shipModel);
    }

    public void CreateTank(Transformable tankModel)
    {
        CreatePresenter(_tankPresenter, tankModel);
    }

    public void CreatePresenter(Presenter presenterTemplate, Transformable model)
    {
        Instantiate(presenterTemplate).Init(model);
    }
}
