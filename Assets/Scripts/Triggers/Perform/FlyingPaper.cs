using System;
using Cysharp.Threading.Tasks;
using Signals;
using Signals.Trigger;
using Triggers.Objects.Fan;
using UnityEngine;


namespace Triggers.Perform
{
    public class FlyingPaper : BaseTriggerPerform
    {
        [Header("Special settings")]
        [SerializeField] private int delay = 5;
        
        private Fan[] _fans;
        private void Awake()
        {
            _fans = GetComponentsInChildren<Fan>();
        }

        protected override async void PlayerTriggerEnter()
        {
            if(_player.IsIgnore) return;
            
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;
            
            TriggerActive(false);
            
            _signal.Fire<FlyingSignal>();
            _signal.Fire(new InfoSignal(nameTrigger, $"Веселье начнется через {delay}сек, беги! чтоб тебя не заметили"));
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            foreach (var fan in _fans)
                fan.StartFlyObjects();
        }

        protected override void PlayerTriggerExit()
        {
            foreach (var fan in _fans)
                fan.ReturnNormalState(); // todo временно
        }
    }
}