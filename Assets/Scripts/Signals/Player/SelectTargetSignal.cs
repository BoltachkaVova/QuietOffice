using Employees;
using Enums;

namespace Signals
{
    public struct SelectTargetSignal
    {
        private EmployeesBase _target;
        private TypeInventory _type;
        public EmployeesBase Target => _target;

        public TypeInventory Type => _type;

        public SelectTargetSignal(EmployeesBase target, TypeInventory type)
        {
            _target = target;
            _type = type;
        }
    }
}