using UnityEngine;
using Zenject;

public class LevelInstallerInstaller : MonoInstaller
{
    [SerializeField] private Transform _dividers;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Grid>().
            AsSingle().
            WithArguments(_dividers.GetComponentsInChildren<PipeDivider>(includeInactive:true)).
            NonLazy();
    }
}