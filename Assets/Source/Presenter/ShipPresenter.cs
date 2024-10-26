public class ShipPresenter : Presenter
{
    public new Ship Model => base.Model as Ship;
    public new ShipView View => base.View as ShipView;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.TryGetComponent(out Offscreen _))
            Model.Destroy();
    }
}
