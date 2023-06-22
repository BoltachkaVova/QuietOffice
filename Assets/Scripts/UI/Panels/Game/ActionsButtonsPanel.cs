using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public enum ButtonsState 
    {
        None,
        StopWork,
        Action,
        Throw
    }

    public class ActionsButtonsPanel : MonoBehaviour
    {
        private ThrowButton _throwButton;
        private StopWorkButton _stopWorkButton;
        private ChangeButton _changeButton;
        private BreakButton _breakButton;
        private PickUpButton _pickUpButton;
        
        private List<Button> _buttonsAll = new List<Button>(10);
        private SignalBus _signal;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
        }

        private void Awake()
        {
            _pickUpButton = GetComponentInChildren<PickUpButton>(true);
            _throwButton = GetComponentInChildren<ThrowButton>(true);
            _stopWorkButton = GetComponentInChildren<StopWorkButton>(true);
            _breakButton = GetComponentInChildren<BreakButton>(true);
            _changeButton = GetComponentInChildren<ChangeButton>(true);

            _buttonsAll = GetComponentsInChildren<Button>(true).ToList();
        }

        private void Start()
        {
            _signal.Subscribe<WorkStateSignal>(OnStartedWork);
            _signal.Subscribe<ShowActionsSignal>(OnActionState);
            
            _signal.Subscribe<LostTargetSignal>(OnLost);
        }

        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkStateSignal>(OnStartedWork);
            _signal.Unsubscribe<ShowActionsSignal>(OnActionState);
            
            _signal.Unsubscribe<LostTargetSignal>(OnLost);
        }
        
        
        private void OnActionState()
        {
            ChangeStateButtons(ButtonsState.Action);
        }

        private async void OnStartedWork()
        {
            ChangeStateButtons(ButtonsState.None);
            await UniTask.Delay(10000); // todo 
            ChangeStateButtons(ButtonsState.StopWork);
        }
        
        private void OnLost()
        { 
            ChangeStateButtons(ButtonsState.None);
        }

        private void ChangeStateButtons(ButtonsState state)
        {
            switch (state)
            {
                case ButtonsState.None:
                    foreach (var button in _buttonsAll)
                        button.gameObject.SetActive(false);
                    break;
                
                case ButtonsState.StopWork:
                    _stopWorkButton.gameObject.SetActive(true);
                    break;
                
                case ButtonsState.Action:
                    _breakButton.gameObject.SetActive(true);
                    _changeButton.gameObject.SetActive(true);
                    _pickUpButton.gameObject.SetActive(true);
                    break;

                case ButtonsState.Throw:
                    _throwButton.gameObject.SetActive(true);
                    break;
                
                default:
                    Debug.Log("ActionPanel Fail");
                    return;
            }
        }
    }
}