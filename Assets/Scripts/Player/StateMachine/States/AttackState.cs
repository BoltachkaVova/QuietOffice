using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Inventory;
using Minions;
using Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Plane = Inventory.Plane;

namespace Player
{
    public class AttackState : IState, IInitializable, IDisposable
    {
        private MinionBase _target;
        private InventoryBase _inventory;
        
        private ThrowPoint _throwPoint;
        
        private float _speedRotate = 7f;
        
        private readonly PlayerAnimator _animator;
        private readonly Joystick _joystick;
        private readonly Player _player;
        private readonly SignalBus _signalBus;

        public AttackState(PlayerAnimator animator, Joystick joystick, Player player, SignalBus signalBus)
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
            _signalBus.Subscribe<TargetLostSignal>(OnTargetLost);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<TargetSelectedSignal>(OnTargetSelected);
            _signalBus.Unsubscribe<TargetLostSignal>(OnTargetLost);
        }

        public void Enter()
        {
            _joystick.OnPointerUp(null);
            _joystick.gameObject.SetActive(false);

            switch (_inventory)
            {
                case Banana:
                    ThrowAtMinion().Forget(); 
                    break;
                case Plane:
                    ThrowObj().Forget();
                    break;
                
                default:
                    Debug.Log($"Fail in PlayerAttackState {_inventory.name}");
                    break;
            }
            
        }

        public void Update()
        {
            if (_target == null)  return;
            
            var direction = (_target.transform.position - _player.transform.position).normalized;
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation,
                Quaternion.LookRotation(direction),
                _speedRotate * Time.deltaTime);
        }

        public async void Exit()
        {
            _joystick.gameObject.SetActive(true);
            
            await UniTask.Delay(2000);
            _target = null;
        }
        
        private async UniTaskVoid ThrowAtMinion()
        {
            _animator.ThrowAtMinion();
            await UniTask.Delay(2000);
            var banan = Object.Instantiate(_inventory, _throwPoint.transform.position, Quaternion.identity);
            await banan.transform.DOJump(_target.transform.position, 4f, 1, 1).SetEase(Ease.InOutSine)
                .Join(banan.transform.DORotate(new Vector3(360,0,360),1.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
            
            _signalBus.Fire<TargetLostSignal>();
        }

        private async UniTaskVoid ThrowObj()
        {
            _animator.ThrowObj();
            await UniTask.Delay(1000);
            Object.Instantiate(_inventory, _throwPoint.transform.position, _throwPoint.transform.rotation);
            
            _signalBus.Fire<TargetLostSignal>();
        }

        private void OnTargetSelected(TargetSelectedSignal selected)
        {
            _target = selected.MinionBase;
            _inventory = selected.InventoryBase;
        }
        
        private void OnTargetLost()
        {
            _target = null;
            _inventory = null;
        }

        
    }
}