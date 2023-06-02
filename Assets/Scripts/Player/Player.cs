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
        private ProgressBar _progressBar;
        
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
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        private void Start()
        {
           // 
        }
        
        private async void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out TriggerBase component))
            {
                _progressBar.Show(component.DurationProgress, component.ViewImage);
                await UniTask.Delay(component.DurationProgress * 1000);
                
                if(!_progressBar.IsActive) return;
                
                switch (component)
                {
                    case WorkTrigger workTrigger:
                        _signalBus.Fire(new WorkSignal(workTrigger.transform, workTrigger.Chair.transform));
                        break;
                    case BananaTrigger bananaTrigger:
                        _signalBus.Fire<TakeSignal>();
                        break;
                    default:
                        Debug.Log(component.name);
                        break;
                }
                
                _signalBus.Fire(new InfoInventorySignal(component.NameInventory, component.TextInfo));
                _informationPanel.gameObject.SetActive(true);
                component.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerBase>())
            {
                _progressBar.Close();
                Debug.Log(other.name);
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