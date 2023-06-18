using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ActionsState : IState, IInitializable, IDisposable
    {
        private Transform _transform;
        private IActions _target;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signal;

        public ActionsState(PlayerAnimator animator, Player player, SignalBus signal)
        {
            _animator = animator;
            _player = player;
            _signal = signal;
        }
        
        public void Initialize()
        {
            _signal.Subscribe<ActionStateSignal>(OnGetPoint);
        }

        public void Dispose()
        {
            _signal.Unsubscribe<ActionStateSignal>(OnGetPoint);
        }
        
        public async void Enter()
        {
            await Move();
            Break();
            
            _signal.Fire<ActiveStateSignal>();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }

        private void Break()
        {
            _target.Break(true); // тип вот сломал
            _target.Change(false); // и тут поменял файлы наверн тип нужно убрать это глупо!!!
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

        private void OnGetPoint(ActionStateSignal signal)
        {
            _transform = signal.TransformObj;
            _target = signal.Actions;
        }

    }
}