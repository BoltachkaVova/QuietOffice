﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class WorkState : IState, IInitializable, IDisposable
    {
        private Transform _triggerTransform;
        private Transform _objTransform;
        private Vector3 _startObjPosition;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signalBus;
        
        public WorkState(PlayerAnimator animator, Player player, SignalBus signalBus)
        {
            _animator = animator;
            _player = player;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<WorkStateSignal>(OnPlayerWork);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<WorkStateSignal>(OnPlayerWork);
        }
        
        public async void Enter()
        {
            await MoveToWorkPoint();
            await StartWork();
        }
        
        public void Update()
        {
            
        }

        public void Exit()
        {
            _animator.StayWork();
            
            var sequence = DOTween.Sequence();
            sequence.Append(_objTransform.DOMove(_startObjPosition, 0.8f)) // todo Мэджик
                .Join(_player.transform.DOMove(_startObjPosition, 0.8f)).Play().SetLoops(0); // todo Мэджик
        }
        
        private void OnPlayerWork(WorkStateSignal obj)
        {
            _triggerTransform = obj.TriggerTransform;
            _objTransform = obj.ObjTransform;
            
            _startObjPosition = obj.ObjTransform.position;
        }
        
        private async UniTask MoveToWorkPoint()
        {
            var rotation = _triggerTransform.rotation;
            var position = _triggerTransform.position;
            
            var sequence = DOTween.Sequence();
            await sequence.Append(_player.transform.DOMove(new Vector3(position.x, 0, position.z), 1f)
                    .OnStart(() => _animator.Move(0.5f)) 
                    .OnComplete(() => _animator.Move(0f)))
                .Append(_player.transform.DORotate(rotation.eulerAngles, 0.5f).SetEase(Ease.Linear));
            // todo добавить что нужно сначала повернуться в сторону триггера!!! и 
        }
        
        private async UniTask StartWork()
        {
            _animator.StartedWork();
            await UniTask.Delay(4100); // todo Мэджик
            await _objTransform.DOMove(_player.transform.position,0.8f); // todo Мэджик
        }
    }
}