using DG.Tweening;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ThrowAtButtonsPanel : MonoBehaviour
    {
        private SelectInventoryButton[] _buttons;
        private Image _image;

        private SignalBus _signal;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
        }

        private void Awake()
        {
            _buttons = GetComponentsInChildren<SelectInventoryButton>(true);
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _signal.Subscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
            
            _signal.Subscribe<ThrowStateSignal>(OnThrow);
        }
        

        private void OnDestroy()
        {
            _signal.Unsubscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);
            
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
        }

        private void OnSelectTarget(SelectTargetSignal target)
        {
            // todo добавить проверку на таргет/ что можно с ним делать? 
            ShowButtons(true);
        }

        private void OnThrow()
        {
            ShowButtons(false);
        }
        
        private void OnTargetLost()
        {
            ShowButtons(false);
        }

        private void ShowButtons(bool isOn)
        {
            foreach (var button in _buttons)
                button.gameObject.SetActive(isOn);
            
            _image.DOFade(isOn ? 0.4f : 0, 0.2f); // todo Мэджик
        }
    }
}