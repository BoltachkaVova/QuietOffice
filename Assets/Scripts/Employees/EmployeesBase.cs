using Enums;
using Interfases;
using UnityEngine;

namespace Employees
{
    public abstract class EmployeesBase : MonoBehaviour, ISelection
    {
        [SerializeField] protected TypeAction[] actionsOn;
        
        private SelectionView selectionView;
        public TypeAction[] ActionsOn => actionsOn;
        
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