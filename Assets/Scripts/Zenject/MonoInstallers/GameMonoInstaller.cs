using Inventory;
using UnityEngine;
using User;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    [Header("Inventory:")]
    [SerializeField] private InventoryBase banana;
    [SerializeField] private InventoryBase regularPlane;
    
    public override void InstallBindings()
    {
        GameInstaller.Install(Container);
        SignalsInstaller.Install(Container);
        
        Container.BindInterfacesTo<InputController>().AsSingle().WithArguments(banana, regularPlane).NonLazy();
    }
}