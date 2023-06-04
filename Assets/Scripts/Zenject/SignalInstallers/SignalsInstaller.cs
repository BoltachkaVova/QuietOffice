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
    }

    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<AttackSignal>();
        Container.DeclareSignal<ExitAttackSignal>();
        
        Container.DeclareSignal<WorkSignal>();
        Container.DeclareSignal<StopWorkSignal>();
        
        Container.DeclareSignal<TargetSelectedSignal>();
        Container.DeclareSignal<TargetLostSignal>();
    }
}