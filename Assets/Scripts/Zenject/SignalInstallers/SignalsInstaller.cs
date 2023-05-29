using Signals;
using UnityEngine;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        BildingPlayerSignals();
    }

    private void BildingPlayerSignals()
    {
        Container.DeclareSignal<WorkSignal>();
    }
}