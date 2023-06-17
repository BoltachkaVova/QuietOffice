using UI;
using Zenject;

public class UIMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UIController>().FromInstance(GetComponent<UIController>()).AsSingle(); 
        BindPanels();
    }

    private void BindPanels()
    {
        Container.Bind<GamePanel>().FromInstance(GetComponentInChildren<GamePanel>(true)).AsSingle();
        Container.Bind<InformationPanel>().FromInstance(GetComponentInChildren<InformationPanel>(true)).AsSingle();
        Container.Bind<InventoryPanel>().FromInstance(GetComponentInChildren<InventoryPanel>(true)).AsSingle();
    }
}