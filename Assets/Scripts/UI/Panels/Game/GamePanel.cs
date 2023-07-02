using Signals;
using UnityEngine;
using Zenject;

namespace UI.Panels.Game
{
    public class GamePanel : MonoBehaviour
    {
        private SignalBus _signal;
        private InventoryButtonsPanel inventoryPanel;
        private ActionsButtonsPanel _actionsPanel;
        
        [Inject]
        public void Construct(SignalBus signalBus, InventoryButtonsPanel inventoryButtonsPanel, 
            ActionsButtonsPanel actionsButtonsPanel)
        {
            _signal = signalBus;
            inventoryPanel = inventoryButtonsPanel;
            _actionsPanel = actionsButtonsPanel;
        }
        

        private void Start()
        {
            _signal.Subscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Subscribe<ScatterHereSignal>(OnScatter);
            
            _signal.Subscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Unsubscribe<ScatterHereSignal>(OnScatter);
            
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnSelectTarget()
        {
            
        }
        
        private void OnScatter()
        {
            
        }
        
        private void OnThrow()
        {
            
        }
    }
    
}