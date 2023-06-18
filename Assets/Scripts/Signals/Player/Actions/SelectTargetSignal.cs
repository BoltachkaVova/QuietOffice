using Employees;
using Enums;

namespace Signals
{
    public struct SelectTargetSignal
    {
        private EmployeesBase _target;

        public EmployeesBase Target => _target;

        public SelectTargetSignal(EmployeesBase target)
        {
            _target = target;
        }
    }
}