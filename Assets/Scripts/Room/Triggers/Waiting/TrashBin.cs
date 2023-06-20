using System;
using System.Collections.Generic;
using Enums;
using Signals;
using UnityEngine;

namespace Room
{
    public class TrashBin : TriggerWaitingBase
    {
        [SerializeField] private List<ConfigTrashBin> trashBins;

        protected override void PlayerTriggerEnter()
        {
            foreach (var trashBin in trashBins)
                _player.AddInventory(trashBin.Count, trashBin.Inventory);
            
            _signal.Fire(new InfoInventorySignal(nameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
            _player.CloseProgress();
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