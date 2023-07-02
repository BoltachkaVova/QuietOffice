using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;

namespace Triggers.Perform
{
    public class Scatter : TriggerPerform
    {
        [SerializeField] private TypeInventory tryScatterInventory;
        protected override async void PlayerTriggerEnter()
        {
            if(!_player.IsIgnore) return;
                
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;
            
            _signal.Fire(new ScatterHereSignal(transform));
            _signal.Fire(new ThrowStateSignal(tryScatterInventory));
            _signal.Fire(new InfoInventorySignal(NameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
            _signal.Fire<LostTargetSignal>();
        }
    }
}