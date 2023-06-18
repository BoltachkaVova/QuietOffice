using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private SignalBus _signal;
        private ThrowAtButtonsPanel _throwAtPanel;
        private ActionsButtonsPanel _actionsPanel;
        
        [Inject]
        public void Construct(SignalBus signalBus, ThrowAtButtonsPanel throwAtButtonsPanel, 
            ActionsButtonsPanel actionsButtonsPanel)
        {
            _signal = signalBus;
            _throwAtPanel = throwAtButtonsPanel;
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