using System;
using Cysharp.Threading.Tasks;
using Inventory;
using Employees;
using Signals;
using Triggers;
using UI;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InventoryBase banana;
        [SerializeField] private InventoryBase regularPlane;
        
        private ThrowPoint _throwPoint;
        private ProgressBar _progressBar;
        
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
      

        private EmployeesBase _target;

        public EmployeesBase Tom => _target;
        public ThrowPoint ThrowPoint => _throwPoint;
        
        public InventoryBase Banana => banana;
        public InventoryBase RegularPlane => regularPlane;


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
                _progressBar.Show(component.DurationProgress, component.ViewImage);
                await UniTask.WaitUntil(() => !_progressBar.IsActive);
                
                if(!_progressBar.IsDone) return;
                
                switch (component)
                {
                    case WorkTrigger workTrigger:
                        _signalBus.Fire(new WorkSignal(workTrigger.transform, workTrigger.Chair.transform));
                        break;
                    
                    case BananaTrigger bananaTrigger:
                        bananaTrigger.gameObject.SetActive(false); 
                        break;
                    
                    default:
                        Debug.Log($"Fail in Player {component.name}");
                        break;
                }
                
                _signalBus.Fire(new InfoInventorySignal(component.NameInventory, component.TextInfo));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerBase>())
                _progressBar.Close(false);
        }

        private void Update() 
        {
            if (Input.GetMouseButtonDown(1))
            {
                _signalBus.Fire(new TargetSelectedSignal(regularPlane));
                ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
            }

            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            var employess = hit.transform.GetComponent<EmployeesBase>();
            if (employess != null)
            {
                _target = employess;
                _target.ThisTarget(true);
                
                _signalBus.Fire(new TargetSelectedSignal(banana, _target));
                ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
            }
        }
        
        
        private async UniTaskVoid ResetTarget() 
        {
            await UniTask.Delay(10000);
            _signalBus.Fire<TargetLostSignal>();
            
            if(_target == null) return;
            
            _target.ThisTarget(false);
            _target = null;
        }

    }
}