using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Signals;
using Room;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform point;
        
        private ThrowPoint _throwPoint;
        private ProgressBar _progressBar;
        
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
        
        public ThrowPoint ThrowPoint => _throwPoint;
        
        private List<OfficeFiles> _officeFileses = new List<OfficeFiles>(20);
        

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
            if (other.TryGetComponent(out TriggerBase component))
            {
                if(!component.IsActive) return;
                
                _progressBar.Show(component.DurationProgress, component.ViewImage);
                await UniTask.WaitUntil(() => !_progressBar.IsActive);
                
                if(!_progressBar.IsDone) return;
                switch (component)
                {
                    case TriggerWork workTrigger:
                        _signalBus.Fire(new WorkStateSignal(workTrigger.transform, workTrigger.Chair.transform));
                        break;
                    
                    case TriggerBanana bananaTrigger:
                        bananaTrigger.gameObject.SetActive(false); 
                        break;
                    
                    case Printer printer:
                        printer.PickUp(point);
                        
                        _signalBus.Fire<IdleStateSignal>();
                        await UniTask.WaitUntil(() => !printer.IsActive);
                        _signalBus.Fire<ActiveStateSignal>();
                        
                        break;
                    
                    default:
                        Debug.Log($"Fail in Player {component.name}");
                        return;
                }
                _signalBus.Fire(new InfoInventorySignal(component.NameInventory, component.TextInfo));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerBase>())
                _progressBar.Close(false);

            if (other.GetComponent<Printer>())
                _officeFileses = GetComponentsInChildren<OfficeFiles>().ToList();
        }
        
    }
}