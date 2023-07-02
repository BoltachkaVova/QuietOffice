using System;
using Employees.Selection;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Employees
{
    public abstract class EmployeesBase : MonoBehaviour, ISelection
    {
        
        private SelectionView selectionView;
        private SignalBus _signal;

        [Inject]
        public void Construct(SignalBus signal)
        {
            _signal = signal;
        }

        private void Awake()
        {
            selectionView = GetComponentInChildren<SelectionView>();
        }

        private void Start()
        {
            _signal.Subscribe<LostTargetSignal>(OnLost);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<LostTargetSignal>(OnLost);
        }
        
        private void OnLost()
        {
            ThisSelection(false);
        }

        public void ThisSelection(bool isOn)
        {
            selectionView.IsOn(isOn);
        }
    }
}