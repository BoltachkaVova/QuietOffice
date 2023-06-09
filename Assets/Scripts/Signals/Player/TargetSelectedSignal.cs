using Inventory;
using Employees;

namespace Signals
{
    public struct TargetSelectedSignal
    {
        private InventoryBase _inventoryBase;
        private EmployeesBase employeesBase;
        
        public InventoryBase InventoryBase => _inventoryBase;
        public EmployeesBase EmployeesBase => employeesBase;

        public TargetSelectedSignal(InventoryBase inventoryBase, EmployeesBase employeesBase = null)
        {
            _inventoryBase = inventoryBase;
            this.employeesBase = employeesBase;
        }
    }
}