using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class BreakState : IState, IInitializable, IDisposable
    {
        private Transform _transform;
        private IActions _target;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signal;

        public BreakState(PlayerAnimator animator, Player player, SignalBus signal)
        {
            _animator = animator;
            _player = player;
            _signal = signal;
        }
        
        public void Initialize()
        {
            _signal.Subscribe<BreakStateSignal>(OnGetPoint);
        }

        public void Dispose()
        {
            _signal.Unsubscribe<BreakStateSignal>(OnGetPoint);
        }
        
        public async void Enter()
        {
            await Move();
            await Break();
            
            _signal.Fire<ActiveStateSignal>();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }

        private async UniTask Break()
        {
            _target.Break(false); // тип вот сломал
            _target.Change(); // и тут поменял файлы наверн тип нужно убрать это глупо!!!
        }
        private async UniTask Move()
        {
            var rotation = _transform.localRotation;
            var position = _transform.position;
            
            _animator.Move(0.5f);
            
            Transform transform;
            await (transform = _player.transform).DOMove(new Vector3(position.x, 0, position.z), 1f)  // todo Мэджик
                .OnComplete(() => _animator.Move(0f));
            
            _player.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2f); // todo Мэджик
        }

        private void OnGetPoint(BreakStateSignal signal)
        {
            _transform = signal.TransformObj;
            _target = signal.Actions;
        }

    }
}