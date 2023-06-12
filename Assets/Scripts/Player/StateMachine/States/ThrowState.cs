using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Inventory;
using Employees;
using Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Player
{
    public class ThrowState : IState, IInitializable, IDisposable
    {
        private EmployeesBase _target;
        private InventoryBase _inventory;
        private ThrowPoint _throwPoint;
        
        private float _speedRotate = 7f;
        private bool _isLookAt;
        
        private readonly PlayerAnimator _animator;
        private readonly Joystick _joystick;
        private readonly Player _player;
        private readonly SignalBus _signalBus;

        public ThrowState(PlayerAnimator animator, Joystick joystick, Player player, SignalBus signalBus)
        {
            _animator = animator;
            _joystick = joystick;
            _player = player;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _throwPoint = _player.ThrowPoint;
            
            _signalBus.Subscribe<TargetSelectedSignal>(OnTargetSelected);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<TargetSelectedSignal>(OnTargetSelected);
        }

        public async void Enter()
        {
            _joystick.OnPointerUp(null);
            _joystick.gameObject.SetActive(false);

            switch (_inventory)
            {
                case Banana:
                    ThrowAtEmployees().Forget();
                    _isLookAt = true;
                    break;
                
                case PaperAirplane:
                    await ThrowObject();
                    break;
                
                default:
                    Debug.Log($"Fail in PlayerAttackState {_inventory.name}");
                    break;
            }
        }

        public void Update()
        {
            if (!_isLookAt)  return;
            LookAt();
        }

        public void Exit()
        {
            _joystick.gameObject.SetActive(true);
        }

        private void LookAt()
        {
            var transform = _player.transform;
            var direction = (_target.transform.position - transform.position).normalized;
            
            _player.transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(direction),
                _speedRotate * Time.deltaTime);
        }

        private async UniTaskVoid ThrowAtEmployees()
        {
            _animator.ThrowAtEmployees();
            await UniTask.Delay(2000);// todo Мэджик
            var banan = Object.Instantiate(_inventory, _throwPoint.transform.position, Quaternion.identity);
            
            await banan.transform.DOJump(_target.transform.position, 4f, 1, 1).SetEase(Ease.InOutSine)
                .Join(banan.transform.DORotate(new Vector3(360,0,360),1.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear); 
            // todo Мэджик
            
            _signalBus.Fire<TargetLostSignal>();
            _isLookAt = false;
        }

        private async UniTask ThrowObject()
        {
            _animator.ThrowObj();
            await UniTask.Delay(900);// todo Мэджик
            
            var transform = _throwPoint.transform;
            Object.Instantiate(_inventory, transform.position, transform.rotation);
            
            _signalBus.Fire<TargetLostSignal>();
        }

        private void OnTargetSelected(TargetSelectedSignal selected)
        {
            _target = selected.EmployeesBase;
            _inventory = selected.InventoryBase;
        }
        
    }
}