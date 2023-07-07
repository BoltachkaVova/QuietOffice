using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;

namespace Triggers.Perform
{
    public class TrashBin : BaseTriggerPerform
    {
        [Header("Special settings")]
        [SerializeField] private List<ConfigTrashBin> trashBins;

        protected override async void PlayerTriggerEnter()
        {
            if(_player.IsIgnore) return;
            
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;

            foreach (var trashBin in trashBins)
                _player.AddInventory(trashBin.Count, trashBin.Inventory);
            
            _signal.Fire(new InfoSignal(nameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
           
        }
        
    }
    
    [Serializable]
    public class ConfigTrashBin
    {
        [SerializeField] private TypeInventory inventory;
        [SerializeField] private int count;
        public TypeInventory Inventory => inventory;
        public int Count => count;
    }
}