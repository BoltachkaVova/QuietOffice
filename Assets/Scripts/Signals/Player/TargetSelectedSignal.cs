using Employees;
using Enums;

namespace Signals
{
    public struct TargetSelectedSignal
    {
        private EmployeesBase _target;
        private TypeInventory _typeInventory;
        
        public EmployeesBase Target => _target;
        public TypeInventory TypeInventory => _typeInventory;

        public TargetSelectedSignal(TypeInventory typeInventory, EmployeesBase target = null)
        {
            _target = target;
            _typeInventory = typeInventory;
        }
    }
}