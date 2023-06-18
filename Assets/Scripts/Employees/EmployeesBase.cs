using Enums;
using Interfases;
using UnityEngine;

namespace Employees
{
    public abstract class EmployeesBase : MonoBehaviour, ISelection
    {
        [SerializeField] protected TypeActionOnEmployees[] actionsOn;
        
        private SelectionView selectionView;

        public TypeActionOnEmployees[] ActionsOn => actionsOn;

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