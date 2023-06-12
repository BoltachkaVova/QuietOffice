using System;
using Cysharp.Threading.Tasks;
using Employees;
using Interfases;
using Inventory;
using Signals;
using UnityEngine;
using Zenject;

namespace User
{
    public class InputController: IInitializable, ITickable, IDisposable
    {
        private EmployeesBase _target;
        
        private readonly InventoryBase _banana;
        private readonly InventoryBase _regularPlane;
        private readonly SignalBus _signalBus;

        public InputController(InventoryBase banana, InventoryBase regularPlane, SignalBus signalBus)
        {
            _banana = banana;
            _regularPlane = regularPlane;
            
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(1))  //  Input.touchCount > 1;
            {
                _signalBus.Fire(new TargetSelectedSignal(_regularPlane));
                ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
            }
            
            if (!Input.GetMouseButtonDown(0)) return;
            CheckSelection();
        }

        public void Dispose()
        {
           
        }
        
        
        private void CheckSelection() 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            var employess = hit.transform.GetComponent<ISelection>();

            if (employess == null) return;
            switch (employess)
            {
                case EmployeesBase target:
                    _target = target;
                    _signalBus.Fire(new TargetSelectedSignal(_banana, _target));
                    break;
                
                default:
                    Debug.Log($"Fail in Player");
                    return;
            }
                
            employess.ThisSelection(true);
            ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
        }
        
        private async UniTaskVoid ResetTarget() 
        {
            await UniTask.Delay(10000); // todo Мэджик
            _signalBus.Fire<TargetLostSignal>();
            
            if(_target == null) return;
            
            _target.ThisSelection(false);
            _target = null;
        }

    }
}