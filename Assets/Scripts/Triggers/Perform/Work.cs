using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;

namespace Triggers.Perform
{
    public class Work : BaseTriggerPerform
    {
        [Header("Special settings")]
        [SerializeField] private Transform chair;
        
        protected override async void PlayerTriggerEnter()
        {
            if(_player.IsIgnore) return;
            
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
            
            if(!_progressBar.IsDone) return;
            
            _signal.Fire(new WorkStateSignal(transform, chair));
            _signal.Fire(new InfoSignal(nameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
            
        }
        
    }
}