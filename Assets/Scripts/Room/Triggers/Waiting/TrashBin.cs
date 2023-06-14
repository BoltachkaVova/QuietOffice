using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Room
{
    public class TrashBin : TriggerWaitingBase
    {
        [SerializeField] private List<InventoryBase> inventory;


        public List<InventoryBase> Inventory => inventory;
    }
}