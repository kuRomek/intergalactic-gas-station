public class ShipPresenter : Presenter
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.TryGetComponent(out Offscreen _))
            Destroy(gameObject);
    }
}
