using Inventory;
using Minions;

namespace Signals
{
    public struct TargetSelectedSignal
    {
        private InventoryBase _inventoryBase;
        private MinionBase _minionBase;
        
        public InventoryBase InventoryBase => _inventoryBase;
        public MinionBase MinionBase => _minionBase;

        public TargetSelectedSignal(InventoryBase inventoryBase, MinionBase minionBase = null)
        {
            _inventoryBase = inventoryBase;
            _minionBase = minionBase;
        }
    }
}