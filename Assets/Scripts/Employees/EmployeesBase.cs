using Interfases;
using UnityEngine;

namespace Employees
{
    public abstract class EmployeesBase : MonoBehaviour, ISelection
    {
        private SelectionView selectionView;

        private void Awake()
        {
            selectionView = GetComponentInChildren<SelectionView>();
        }

        public void ThisSelection(bool isOn)
        {
            selectionView.IsOn(isOn);
        }
    }
}