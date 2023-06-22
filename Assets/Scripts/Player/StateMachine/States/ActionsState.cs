using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Signals;
using Triggers;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ActionsState : IState, IInitializable, IDisposable
    {
        private Transform _transform;
        private TriggerAction _trigger;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signal;
        private readonly ProgressBar _progressBar;

        public ActionsState(PlayerAnimator animator, Player player, SignalBus signal, 
            ProgressBar progressBar)
        {
            _animator = animator;
            _player = player;
            _signal = signal;
            _progressBar = progressBar;
        }
        
        public void Initialize()
        {
            _signal.Subscribe<SelectTriggerActionSignal>(OnGetSettings);
            _signal.Subscribe<BreakSignal>(OnBreak);
            _signal.Subscribe<ChangeSignal>(OnChange);
            _signal.Subscribe<PickUpSignal>(OnPickUp);
        }
        
        public void Dispose()
        {
            _signal.Unsubscribe<SelectTriggerActionSignal>(OnGetSettings);
            _signal.Unsubscribe<BreakSignal>(OnBreak);
            _signal.Unsubscribe<ChangeSignal>(OnChange);
            _signal.Unsubscribe<PickUpSignal>(OnPickUp);
        }
        
        public void Enter()
        {
            _signal.Fire<LostTargetSignal>();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
        
        private async void OnPickUp()
        {
            _trigger.PickUp(_player.PickUpPoint);
            await UniTask.WaitWhile(()=> _trigger.IsActiveTrigger);
            
            _signal.Fire<ActiveStateSignal>();
        } 

        private async void OnChange()
        {
            await Move();
            await _trigger.Change();
            
            _signal.Fire(new InfoInventorySignal(_trigger.NameTrigger, "Измененя прошли успешно!"));
            _signal.Fire<ActiveStateSignal>();
        }

        private async void OnBreak()
        {
            await Move();
            await _trigger.Break(); 
            
            _signal.Fire(new InfoInventorySignal(_trigger.NameTrigger, "Ты его сломал!"));
            _signal.Fire<ActiveStateSignal>();
        }
        
        private async UniTask Move()
        {
            var position = _transform.position;
            
            var direction = position -  _player.transform.position;
            var rotation = Quaternion.LookRotation(direction);
            
            await _player.transform.DORotateQuaternion(rotation, 0.4f).SetEase(Ease.Linear);
            await _player.transform.DOMove(new Vector3(position.x, 0, position.z), 1f)
                .OnStart(()=>_animator.Move(0.5f)).OnComplete(() => _animator.Move(0f));
            
        }

        private void OnGetSettings(SelectTriggerActionSignal signal)
        {
            _trigger = signal.Action;
            _transform = signal.Target;
        }

    }
}