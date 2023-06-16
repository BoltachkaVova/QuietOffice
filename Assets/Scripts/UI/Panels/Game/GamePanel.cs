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
        private InformationPanel _informationPanel; // todo entity 

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
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
            
            _signal.Subscribe<SelectedSignal>(OnTargetSelect);
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkStateSignal>(OnWork); 
            _signal.Unsubscribe<ActiveStateSignal>(OnActive);
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
            _signal.Unsubscribe<IdleStateSignal>(OnIdle);
            
            _signal.Unsubscribe<SelectedSignal>(OnTargetSelect);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);
        }
        
        private async void OnWork()
        {
            await UniTask.Delay(10000);
            ChangeButtons(TypeButton.StopWork);
        }
        
        private void OnActive()
        {
            ChangeButtons(TypeButton.None);
            Debug.Log("UI Active");
        }
        
        private void OnThrow()
        {
            ChangeButtons(TypeButton.None);
        }
        
        private void OnIdle()
        {
           ChangeButtons(TypeButton.None);
        }
        
        private void OnTargetLost()
        {
            ChangeButtons(TypeButton.None);
        }

        private void OnTargetSelect()
        {
            ChangeButtons(TypeButton.Throw); // todo добавить еще возможности что-то делать с целью
        }

        private void ChangeButtons(params TypeButton[] typeButton) 
        {
            foreach (var type in typeButton)
            {
                _throwButton.gameObject.SetActive(TypeButton.Throw == type);
                _stopWorkButton.gameObject.SetActive(TypeButton.StopWork == type);
            }
        }
    }

    public enum TypeButton
    {
        None,
        Throw,
        StopWork
    }
}