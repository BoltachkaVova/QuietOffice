using Signals;
using Signals.Trigger;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        PlayerSignals();
        TextSignals();
        TriggersSignals();
    }

    private void TextSignals()
    {
        Container.DeclareSignal<InfoSignal>();
    }
    
    private void PlayerSignals()
    {
        Container.DeclareSignal<WorkStateSignal>();
        Container.DeclareSignal<ActiveStateSignal>();
        Container.DeclareSignal<ThrowStateSignal>();
        Container.DeclareSignal<IdleStateSignal>();
        Container.DeclareSignal<ActionTriggerStateSignal>();
        
        Container.DeclareSignal<SelectTargetSignal>();
        Container.DeclareSignal<SelectTriggerActionSignal>();
        Container.DeclareSignal<LostTargetSignal>();

        Container.DeclareSignal<ShowActionsSignal>();
        Container.DeclareSignal<ChangeSignal>();
        Container.DeclareSignal<BreakSignal>();
        Container.DeclareSignal<PickUpSignal>();
        Container.DeclareSignal<ScatterHereSignal>();
    }

    private void TriggersSignals()
    {
        Container.DeclareSignal<ScatterSignal>();
        Container.DeclareSignal<FlyingSignal>();
        Container.DeclareSignal<TrashBinSignal>();
       
    }
}