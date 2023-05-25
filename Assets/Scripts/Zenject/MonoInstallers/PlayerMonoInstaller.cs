using Player;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<PlayerAnimator>().AsSingle().WithArguments(GetComponentInChildren<Animator>());
        Container.Bind<Player.Player>().FromInstance(GetComponent<Player.Player>());
        
        PlayerInstaller.Install(Container);
    }
}