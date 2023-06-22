using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enums;
using Signals;
using UnityEngine;

namespace Triggers
{
    public class TrashBin : TriggerPerform
    {
        [SerializeField] private List<ConfigTrashBin> trashBins;

        protected override async void PlayerTriggerEnter()
        {
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
           
            if(!_progressBar.IsDone) return;

            foreach (var trashBin in trashBins)
                _player.AddInventory(trashBin.Count, trashBin.Inventory);
            
            _signal.Fire(new InfoInventorySignal(NameTrigger, textInfo));
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