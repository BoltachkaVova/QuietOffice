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
        private readonly Joystick _joystick;

        public WorkState(PlayerAnimator animator, Player player, SignalBus signalBus, Joystick joystick)
        {
            _animator = animator;
            _player = player;
            _signalBus = signalBus;
            _joystick = joystick;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<WorkSignal>(OnPlayerWork);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<WorkSignal>(OnPlayerWork);
        }
        
        public async void Enter()
        {
            _joystick.gameObject.SetActive(false);
            _joystick.OnPointerUp(null);
            
            var rotation = _triggerTransform.localRotation;
            var position = _triggerTransform.position;

            await _player.transform.DOMove(new Vector3(position.x, 0, position.z), 0.5f);
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, rotation, 2f);
            
            _animator.StartedWork();
            
            await UniTask.Delay(4000);
            _objTransform.DOMove(_player.transform.position,0.8f);
        }

        public void Update()
        {
            
        }

        public async void Exit()
        {
            Sequence sequence = DOTween.Sequence(); 
            sequence.Append(_objTransform.DOMove(_startObjPosition, 0.8f))
                .Join(_player.transform.DOMove(_startObjPosition, 0.8f)).Play().SetLoops(0);
            
            _animator.StayWork();
            await UniTask.Delay(990);
            
            _joystick.gameObject.SetActive(true);
        }
        
        private void OnPlayerWork(WorkSignal obj)
        {
            _triggerTransform = obj.TriggerTransform;
            _objTransform = obj.ObjTransform;
            
            _startObjPosition = obj.ObjTransform.position;
        }
    }
}