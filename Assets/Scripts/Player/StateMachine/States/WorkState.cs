using System;
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
        private GameObject _gameObject;
        
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
            _joystick.OnPointerUp(null);
            _joystick.gameObject.SetActive(false);
            
            var rotation = _gameObject.transform.localRotation;
            var position = _gameObject.transform.position;

            await _player.transform.DOMove(new Vector3(position.x, 0, position.z), 1f);
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, rotation, 2f);

            _animator.Work();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _joystick.gameObject.SetActive(true);
        }
        
        private void OnPlayerWork(WorkSignal obj)
        {
            _gameObject = obj.Component;
        }
    }
}