using Zenject;

public class LevelInstallerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Grid>().AsSingle().NonLazy();
    }
}