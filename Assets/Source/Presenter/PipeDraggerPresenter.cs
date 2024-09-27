public class PipeDraggerPresenter : Presenter
{
    public new PipeDragger Model => base.Model as PipeDragger;

    private void OnEnable()
    {
        Model.Enable();
    }

    private void OnDisable()
    {
        Model.Disable();
    }
}
