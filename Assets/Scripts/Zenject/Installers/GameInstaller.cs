using QuietOffice.Camera;
using Zenject;

public class GameInstaller : Installer<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TrackingCamera>().AsSingle().NonLazy();
    }
}