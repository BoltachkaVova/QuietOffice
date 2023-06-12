using QuietOffice.Camera;
using User;
using Zenject;

public class GameInstaller : Installer<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TrackingCamera>().AsSingle().NonLazy();
        
    }
}