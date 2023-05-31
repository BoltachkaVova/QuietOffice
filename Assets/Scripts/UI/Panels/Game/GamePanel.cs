using System;
using Signals;
using UnityEngine;
using Zenject;


namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private TakeButton _takeButton;
        private ThrowButton _throwButton;

        private SignalBus _signal;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
        }

        private void Awake()
        {
            _throwButton = GetComponentInChildren<ThrowButton>(true);
            _takeButton = GetComponentInChildren<TakeButton>(true);
        }

        private void Start()
        {
            _signal.Subscribe<TakeSignal>(OnShowButton);
        }

        private void OnDestroy()
        {
            _signal.Subscribe<TakeSignal>(OnShowButton);
        }

        private void OnShowButton()
        {
            _throwButton.gameObject.SetActive(true); // todo временно
        }
    }
}