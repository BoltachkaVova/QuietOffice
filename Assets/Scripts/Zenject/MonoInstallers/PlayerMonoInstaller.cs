using Inventory;
using Player;
using Pool;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private InventoryBase bananaPrefab;
    [SerializeField] private InventoryBase airplanePrefab;
    public override void InstallBindings()
    {
        Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();
       // Container.Bind<Pool<InventoryBase>>().AsSingle().WithArguments().NonLazy();
        
        Container.Bind<PlayerAnimator>().AsSingle().WithArguments(GetComponentInChildren<Animator>());
        Container.Bind<Player.Player>().FromInstance(GetComponent<Player.Player>());
        
        
        Container.BindInterfacesAndSelfTo<ThrowState>().AsSingle().WithArguments(bananaPrefab, airplanePrefab);
        PlayerInstaller.Install(Container);
    }
}