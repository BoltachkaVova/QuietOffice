using System;
using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;

namespace Triggers.Perform
{
    public class Scatter : BaseTriggerPerform
    {
        [Header("Special settings")]
        [SerializeField] private TypeInventory tryScatterInventory;
        [SerializeField] private Transform scatterHere;

        private void Start()
        {
            TriggerActive(false);
        }

        protected override async void PlayerTriggerEnter()
        {
            if(!_player.IsIgnore) return;
                
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;
            
            _signal.Fire(new ScatterHereSignal(scatterHere));
            _signal.Fire(new ThrowStateSignal(tryScatterInventory));
            _signal.Fire(new InfoSignal(nameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
            
        }
    }
}