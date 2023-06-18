using Signals;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        BildingPlayerSignals();
        BildingTextSignals();
    }

    private void BildingTextSignals()
    {
        Container.DeclareSignal<InfoInventorySignal>();
    }

   
    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<WorkStateSignal>();
        Container.DeclareSignal<ActiveStateSignal>();
        Container.DeclareSignal<ThrowStateSignal>();
        Container.DeclareSignal<IdleStateSignal>();
        Container.DeclareSignal<ActionStateSignal>();
        
        Container.DeclareSignal<SelectTargetSignal>();
        Container.DeclareSignal<TargetLostSignal>();
        
        Container.DeclareSignal<ScatterHereSignal>();
    }
}