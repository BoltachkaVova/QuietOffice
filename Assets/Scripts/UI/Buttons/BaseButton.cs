using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public abstract class BaseButton<T> : MonoBehaviour where T : struct 
    {
        private Button _button;
        protected SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        protected virtual void OnClick()
        {
            _signalBus.Fire<T>();
        }
    }
}