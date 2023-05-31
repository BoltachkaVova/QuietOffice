using Signals;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        BildingButtonSignals();
        BildingPlayerSignals();
    }

    private void BildingButtonSignals()
    {
        Container.DeclareSignal<OpenCloseInventorySignal>();
        Container.DeclareSignal<ThrowSignal>();
        Container.DeclareSignal<TakeSignal>();
    }

    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<WorkSignal>();
    }
}