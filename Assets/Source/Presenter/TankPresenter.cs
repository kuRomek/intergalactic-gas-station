using UnityEngine;

[RequireComponent(typeof(TankView))]
public class TankPresenter : Presenter
{
    public new Tank Model => base.Model as Tank;
    public new TankView View => base.View as TankView;
}
