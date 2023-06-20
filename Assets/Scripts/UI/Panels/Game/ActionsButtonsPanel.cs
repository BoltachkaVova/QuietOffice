using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ActionsButtonsPanel : MonoBehaviour
    {
        private ThrowButton _throwButton;
        private StopWorkButton _stopWorkButton;
        
        private SignalBus _signal;

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
            _signal.Subscribe<WorkStateSignal>(OnStartedWork);
            _signal.Subscribe<ActiveStateSignal>(OnStopWork);
            
            _signal.Subscribe<ScatterHereSignal>(OnScatter);
            _signal.Subscribe<TargetLostSignal>(OnLost);
        }
        

        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkStateSignal>(OnStartedWork);
            _signal.Unsubscribe<ActiveStateSignal>(OnStopWork);
            
            _signal.Unsubscribe<ScatterHereSignal>(OnScatter);
            _signal.Unsubscribe<TargetLostSignal>(OnLost);
        }

        private void OnStopWork()
        {
            ChangeButtons(TypeAction.None);
        }

        private void OnScatter()
        {
            ChangeButtons(TypeAction.Throw);
        }

        private async void OnStartedWork()
        {
            await UniTask.Delay(10000);
            ChangeButtons(TypeAction.StopWork);
        }
        
        private void OnLost()
        {
            ChangeButtons(TypeAction.None);
        }

        private void ChangeButtons(TypeAction type)
        {
            _throwButton.gameObject.SetActive(TypeAction.Throw == type);
            _stopWorkButton.gameObject.SetActive(TypeAction.StopWork == type);
        }
    }
}