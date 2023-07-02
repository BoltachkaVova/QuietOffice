using Signals;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        PlayerSignals();
        TextSignals();
    }

    private void TextSignals()
    {
        Container.DeclareSignal<InfoInventorySignal>();
    }

   
    private void PlayerSignals()
    {
        Container.DeclareSignal<WorkStateSignal>();
        Container.DeclareSignal<ActiveStateSignal>();
        Container.DeclareSignal<ThrowStateSignal>();
        Container.DeclareSignal<IdleStateSignal>();
        Container.DeclareSignal<ActionStateSignal>();
        
        Container.DeclareSignal<SelectTargetSignal>();
        Container.DeclareSignal<SelectTriggerActionSignal>();
        Container.DeclareSignal<LostTargetSignal>();
        
        Container.DeclareSignal<ShowActionsSignal>();
        Container.DeclareSignal<ScatterHereSignal>();
        Container.DeclareSignal<ChangeSignal>();
        Container.DeclareSignal<BreakSignal>();
        Container.DeclareSignal<PickUpSignal>();
    }
}