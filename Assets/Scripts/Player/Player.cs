using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Inventory;
using Signals;
using Triggers;
using UI;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        private ThrowPoint _throwPoint;

        private List<GameObject> _inventory = new List<GameObject>(10);
        
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
        private InformationPanel _informationPanel;

        [Inject]
        public void Construct(PlayerAnimator animator, Joystick joystick, SignalBus signalBus, InformationPanel informationPanel)
        {
            _animator = animator;
            _joystick = joystick;
            _signalBus = signalBus;
            _informationPanel = informationPanel;
        }

        private void Awake()
        {
            _throwPoint = GetComponentInChildren<ThrowPoint>();
        }

        private void Start()
        {
            _signalBus.Subscribe<ThrowSignal>(OnThrow);
        }
        
        private async void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Work component))
            {
                //todo добавить задержку и прогресс бар
                _signalBus.Fire(new WorkSignal(component.transform, component.Chair.transform));
            }

            if (other.TryGetComponent(out Banana banana))
            {
                _informationPanel.gameObject.SetActive(true);
                
                _inventory.Add(banana.gameObject);
                banana.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<ThrowSignal>(OnThrow);
        }
        
        private async void OnThrow()
        {
            _animator.StartThrow();
        }
    }
}