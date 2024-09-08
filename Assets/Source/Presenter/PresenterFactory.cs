using UnityEngine;

public class PresenterFactory : MonoBehaviour
{
    [SerializeField] private ShipPresenter _shipPresenter;

    public void CreateShip(Transformable shipModel)
    {
        CreatePresenter(_shipPresenter, shipModel);
    }

    public void CreatePresenter(Presenter presenterTemplate, Transformable model)
    {
        Instantiate(presenterTemplate).Init(model);
    }
}
