using QuietOffice;
using Zenject;

public class ProjectInstaller : Installer<ProjectInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameEngine>().AsSingle().NonLazy();
    }
}