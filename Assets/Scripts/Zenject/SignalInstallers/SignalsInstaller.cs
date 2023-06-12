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
    }

    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<WorkStateSignal>();
        Container.DeclareSignal<ActiveStateSignal>();
        
        Container.DeclareSignal<TargetSelectedSignal>();
        Container.DeclareSignal<TargetLostSignal>();
        
        Container.DeclareSignal<IdleStateSignal>();
        
        
        
    }
}