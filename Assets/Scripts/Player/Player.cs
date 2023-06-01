using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Inventory;
using QuietOffice;
using Signals;
using Triggers;
using UI;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Banana banana;
        [SerializeField] private Tom tom;
        
        private ThrowPoint _throwPoint;
        
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
        private InformationPanel _informationPanel;

        private Tom _target;

        public Tom Tom1 => tom;
        public ThrowPoint ThrowPoint => _throwPoint;
        public Banana Banana => banana;


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
           // 
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Work component))
            {
                //todo добавить задержку и прогресс бар
                _signalBus.Fire(new WorkSignal(component.transform, component.Chair.transform));
            }

            if (other.TryGetComponent(out Banana banana))
            {
                _signalBus.Fire<TakeSignal>();
                _signalBus.Fire(new InfoInventorySignal(banana.NameInventory, banana.TextInfo));
                _informationPanel.gameObject.SetActive(true);
                banana.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            
        }

        private void OnDestroy()
        {
            //
        }
        
       
        
    }
}