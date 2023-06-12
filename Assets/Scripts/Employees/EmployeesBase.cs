using Interfases;
using UnityEngine;

namespace Employees
{
    public abstract class EmployeesBase : MonoBehaviour, ISelection
    {
        private Selection _selection;

        private void Awake()
        {
            _selection = GetComponentInChildren<Selection>();
        }

        public void ThisSelection(bool isOn)
        {
            _selection.IsOn(isOn);
        }
    }
}