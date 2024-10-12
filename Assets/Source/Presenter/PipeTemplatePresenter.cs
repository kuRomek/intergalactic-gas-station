using UnityEngine;

[RequireComponent(typeof(PipeTemplateView))]
public class PipeTemplatePresenter : Presenter
{
    [SerializeField] private Fuel _fuelType;

    public new PipeTemplate Model => base.Model as PipeTemplate;
    public new PipeTemplateView View => base.View as PipeTemplateView;
    public Fuel FuelType => _fuelType;

    private void OnValidate()
    {
        GetComponent<PipeTemplateView>().Init(FuelType);
    }
}
