using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enums;
using Inventory;
using Signals;
using Room;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform transformPoint;
        
        private ThrowPoint _throwPoint;
        private ProgressBar _progressBar;
        
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
        
        private bool _isBusy = false;
        private List<OfficeFiles> _officeFileses = new List<OfficeFiles>(20);
        
        public ThrowPoint ThrowPoint => _throwPoint;
       
        public bool IsBusy => _isBusy;
        public List<OfficeFiles> OfficeFileses => _officeFileses;
        

        [Inject]
        public void Construct(PlayerAnimator animator, Joystick joystick, SignalBus signalBus)
        {
            _animator = animator;
            _joystick = joystick;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _throwPoint = GetComponentInChildren<ThrowPoint>();
            _progressBar = GetComponentInChildren<ProgressBar>();
        }
        

        private async void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out TriggerWaitingBase component))
            {
                if(!component.IsActive || _isBusy) return;
                _progressBar.Show(component.DurationProgress, component.ViewImage);
                
                await UniTask.WaitUntil(() => !_progressBar.IsActive);
                
                if(!_progressBar.IsDone) return;
                switch (component)
                {
                    case TriggerWaitingWork workTrigger:
                        _signalBus.Fire(new WorkStateSignal(workTrigger.transform, workTrigger.Chair.transform));
                        break;
                    
                    case TriggerWaitingBanana bananaTrigger:
                        bananaTrigger.gameObject.SetActive(false); 
                        break;
                    
                    case TriggerWaitingPrinter printer:
                        _signalBus.Fire<IdleStateSignal>();
                        printer.PickUp(transformPoint);
                        
                        await UniTask.WaitUntil(() => !printer.IsActive);
                        _signalBus.Fire<ActiveStateSignal>();
                        _isBusy = true;
                        break;
                    
                    default:
                        Debug.Log($"Fail in Player {component.name}");
                        return;
                }
                _signalBus.Fire(new InfoInventorySignal(component.NameTrigger, component.TextInfo));
            }
            
            
            if (other.TryGetComponent(out TriggerWaitingScatter floor) && _isBusy)  
            {
                _signalBus.Fire(new BusySignal(floor.transform));
                _signalBus.Fire(new TargetSelectedSignal(TypeInventory.Files));
                _signalBus.Subscribe<ThrowStateSignal>(OnThrowSignal);
            }
            
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerWaitingBase>())
                _progressBar.Close(false);

            if (other.GetComponent<TriggerWaitingPrinter>())
                _officeFileses = GetComponentsInChildren<OfficeFiles>().ToList();
        }
        

        private void OnThrowSignal()
        {
            _isBusy = false;
            _officeFileses.Clear();
            
            _signalBus.Unsubscribe<ThrowStateSignal>(OnThrowSignal);
        }
        
    }
}