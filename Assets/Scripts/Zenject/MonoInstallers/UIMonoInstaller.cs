using UI;
using Zenject;

public class UIMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
       BindPanels();
    }

    private void BindPanels()
    {
        Container.Bind<GamePanel>().FromInstance(GetComponentInChildren<GamePanel>(true)).AsSingle();
        Container.Bind<InformationPanel>().FromInstance(GetComponentInChildren<InformationPanel>(true)).AsSingle();
        
        Container.Bind<UIManager>().FromInstance(GetComponent<UIManager>()).AsSingle();
    }
}