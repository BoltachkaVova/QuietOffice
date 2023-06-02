using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Inventory;
using QuietOffice;
using Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Player
{
    public class AttackState : IState, IInitializable, IDisposable
    {
        private Tom _target;
        private Banana _banana;
        private ThrowPoint _throwPoint;
        
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
            _banana = _player.Banana;
            _throwPoint = _player.ThrowPoint;
            
            _signalBus.Subscribe<ThrowSignal>(OnThrow);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ThrowSignal>(OnThrow);
        }

        public void Enter()
        {
            _joystick.OnPointerUp(null);
            _joystick.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (_target != null)
            {
                var direction = (_target.transform.position - _player.transform.position).normalized;
                _player.transform.rotation = Quaternion.LookRotation(direction);
            }

            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            
            var minion = hit.transform.GetComponent<Tom>();
            if (minion != null)
                _target = minion;
        }

        public async void Exit()
        {
            _joystick.gameObject.SetActive(true);
            
            await UniTask.Delay(2000);
            _target = null;
        }
        
        private async void OnThrow()
        {
            if(_target == null) return;

            _animator.StartThrow();
            await UniTask.Delay(800);
            var banan = Object.Instantiate(_banana, _throwPoint.transform.position, Quaternion.identity);
            await banan.transform.DOJump(_target.transform.position, 5f, 1, 1).SetEase(Ease.InOutSine)
                .Join(banan.transform.DORotate(new Vector3(360,0,360),1.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
        }

        
    }
}