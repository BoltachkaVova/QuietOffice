using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        GameInstaller.Install(Container);
        SignalsInstaller.Install(Container);
    }
}