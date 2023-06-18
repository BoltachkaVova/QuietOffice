using System;
using System.Collections.Generic;
using Enums;
using Inventory;
using UnityEngine;

namespace Room
{
    public class TrashBin : TriggerWaitingBase
    {
        [SerializeField] private List<ConfigTrashBin> trashBins;
        public List<ConfigTrashBin> TrashBins => trashBins;
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