using Player;
using Zenject;

public class PlayerInstaller : Installer<PlayerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ActiveState>().AsSingle();
        Container.Bind<IdleState>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<WorkState>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThrowState>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
    }
}