using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;

namespace Triggers
{
    public class Scatter : TriggerPerform
    {
        [SerializeField] private TypeInventory tryScatterInventory;
        protected override async void PlayerTriggerEnter()
        {
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;
            
            _signal.Fire(new ScatterHereSignal(transform));
            _signal.Fire(new ThrowStateSignal(tryScatterInventory));
        }

        protected override void PlayerTriggerExit()
        {
            _signal.Fire<LostTargetSignal>();
        }
    }
}