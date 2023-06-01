using Signals;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        BildingButtonSignals();
        BildingPlayerSignals();
        BildingTextSignals();
    }

    private void BildingTextSignals()
    {
        Container.DeclareSignal<InfoInventorySignal>();
    }

    private void BildingButtonSignals()
    {
        Container.DeclareSignal<OpenCloseInventorySignal>();
        Container.DeclareSignal<ThrowSignal>();
        Container.DeclareSignal<TakeSignal>();
        Container.DeclareSignal<StopWorkSignal>();
    }

    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<WorkSignal>();
        Container.DeclareSignal<AttackSignal>();
    }
}