using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private ThrowButton _throwButton;
        private StopWorkButton _stopWorkButton;

        private SignalBus _signal;
        private InventoryPanel _inventoryPanel;
        

        [Inject]
        public void Construct(SignalBus signalBus, InventoryPanel inventoryPanel)
        {
            _signal = signalBus;
            _inventoryPanel = inventoryPanel;
        }

        private void Awake()
        {
            _throwButton = GetComponentInChildren<ThrowButton>(true);
            _stopWorkButton = GetComponentInChildren<StopWorkButton>(true);
        }

        private void Start()
        {
            _signal.Subscribe<WorkStateSignal>(OnWork);
            _signal.Subscribe<ActiveStateSignal>(OnActive);
            _signal.Subscribe<ThrowStateSignal>(OnThrow);
            _signal.Subscribe<IdleStateSignal>(OnIdle);
            
            _signal.Subscribe<SelectTargetSignal>(OnTargetSelect);
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkStateSignal>(OnWork); 
            _signal.Unsubscribe<ActiveStateSignal>(OnActive);
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
            _signal.Unsubscribe<IdleStateSignal>(OnIdle);
            
            _signal.Unsubscribe<SelectTargetSignal>(OnTargetSelect);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);
        }
        
        private async void OnWork()
        {
            await UniTask.Delay(10000);
            ChangeButtons(TypeButtons.StopWork);
        }
        
        private void OnActive()
        {
            ChangeButtons(TypeButtons.None);
        }
        
        private void OnThrow()
        {
            ChangeButtons(TypeButtons.None);
        }
        
        private void OnIdle()
        {
           ChangeButtons(TypeButtons.None);
        }
        
        private void OnTargetLost()
        {
            ChangeButtons(TypeButtons.None);
        }

        private void OnTargetSelect(SelectTargetSignal select)
        {
            if(select.Target == null)
                ChangeButtons(TypeButtons.Throw);
            else
                _inventoryPanel.gameObject.SetActive(true);
        }

        private void ChangeButtons(params TypeButtons[] typeButton) 
        {
            foreach (var type in typeButton)
            {
                _throwButton.gameObject.SetActive(TypeButtons.Throw == type);
                _stopWorkButton.gameObject.SetActive(TypeButtons.StopWork == type);
            }
        }
    }

    public enum TypeButtons
    {
        None,
        Throw,
        StopWork
    }
}