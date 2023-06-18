using System;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    
    public class ActionsButtonsPanel : MonoBehaviour
    {
        private enum TypeButtons
        {
            None,
            Throw,
            StopWork
        }

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
            ChangeButtons(TypeButtons.None);
        }

        private void OnScatter()
        {
            ChangeButtons(TypeButtons.Throw);
        }

        private async void OnStartedWork()
        {
            await UniTask.Delay(10000);
            ChangeButtons(TypeButtons.StopWork);
        }
        
        private void OnLost()
        {
            ChangeButtons(TypeButtons.None);
        }

        private void ChangeButtons(TypeButtons type)
        {
            _throwButton.gameObject.SetActive(TypeButtons.Throw == type);
            _stopWorkButton.gameObject.SetActive(TypeButtons.StopWork == type);
        }
    }
}