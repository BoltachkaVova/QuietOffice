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
        private InformationPanel _informationPanel;

        private Tom _target;

        public Tom Tom => _target;
        public ThrowPoint ThrowPoint => _throwPoint;
        
        public InventoryBase Banana => banana;
        public InventoryBase RegularPlane => regularPlane;


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
                        Debug.Log($"Fail in Player {component.name}");
                        break;
                }
                
                _signalBus.Fire(new InfoInventorySignal(component.NameInventory, component.TextInfo));
                _informationPanel.gameObject.SetActive(true); // todo  поправить 
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
            if (Input.GetMouseButtonDown(1))
            {
                _signalBus.Fire(new TargetSelectedSignal(regularPlane));
                ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
            }

            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            
            var minion = hit.transform.GetComponent<Tom>();

            if (minion != null)
            {
                _target = minion;
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