using Inventory;
using Player;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private InventoryBase[] inventoryPrefabs;
    
    public override void InstallBindings()
    {
        Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();

        Container.Bind<PlayerAnimator>().AsSingle().WithArguments(GetComponentInChildren<Animator>());
        Container.Bind<Player.Player>().FromInstance(GetComponent<Player.Player>());
        
        
        Container.BindInterfacesAndSelfTo<ThrowState>().AsSingle().WithArguments(inventoryPrefabs);
        PlayerInstaller.Install(Container);
    }
}