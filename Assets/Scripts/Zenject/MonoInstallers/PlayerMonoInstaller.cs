using Inventory;
using Player;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private InventoryBase[] inventoryPrefabs;
    [SerializeField] private ProgressBar progressBar;
    
    public override void InstallBindings()
    {
        Container.Bind<Joystick>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerAnimator>().AsSingle().WithArguments(GetComponentInChildren<Animator>());
        Container.Bind<ProgressBar>().FromInstance(progressBar).AsSingle();
        Container.Bind<Player.Player>().FromInstance(GetComponent<Player.Player>()).AsSingle();
        
        Container.BindInterfacesAndSelfTo<ThrowState>().AsSingle().WithArguments(inventoryPrefabs);
        PlayerInstaller.Install(Container);
    }
}