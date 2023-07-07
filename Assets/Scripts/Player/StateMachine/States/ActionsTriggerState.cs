using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Signals;
using Triggers.Action;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ActionsTriggerState : IState, IInitializable, IDisposable
    {
        private Transform _transform;
        private BaseTriggerAction baseTrigger;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signal;
        private readonly ProgressBar _progressBar;

        public ActionsTriggerState(PlayerAnimator animator, Player player, SignalBus signal, 
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
            _player.OnIgnore(true);
            
            baseTrigger.PickUp(_player.PickUpPoint);
            await UniTask.WaitWhile(()=> baseTrigger.IsActiveTrigger);
            
            _signal.Fire<ActiveStateSignal>();
        } 

        private async void OnChange()
        {
            await Move();
            await baseTrigger.Change();
            
            _signal.Fire(new InfoSignal(baseTrigger.NameTrigger, "Измененя прошли успешно!"));
            _signal.Fire<ActiveStateSignal>();
        }

        private async void OnBreak()
        {
            await Move();
            await baseTrigger.Break(); 
            
            _signal.Fire(new InfoSignal(baseTrigger.NameTrigger, "Ты его сломал!"));
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
            baseTrigger = signal.Action;
            _transform = signal.Target;
        }

    }
}