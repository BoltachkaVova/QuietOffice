using Player;
using Zenject;

public class PlayerInstaller : Installer<PlayerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ActiveState>().AsSingle();
        Container.BindInterfacesAndSelfTo<WorkState>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
    }
}