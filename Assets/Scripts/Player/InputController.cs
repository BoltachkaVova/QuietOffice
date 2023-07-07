using System;
using Cysharp.Threading.Tasks;
using Employees;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class InputController: ITickable, IInitializable, IDisposable
    {
        private EmployeesBase _target;

        private int clickCount = 0;
        private float doubleClickTime = 0.2f;

        private readonly SignalBus _signalBus;
        private readonly Player _player;

        public InputController(SignalBus signalBus, Player player)
        {
            _player = player;
            _signalBus = signalBus;
        }
        public void Initialize()
        {
            
        }
        
        public void Tick()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            clickCount++;
            if (clickCount == 1)
                HandleClicksAsync().Forget();
        }
        
        public void Dispose()
        {
           
        }
        
        private async UniTaskVoid HandleClicksAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(doubleClickTime));

            if (clickCount >= 2) 
            {
                if (_player.IsIgnore) return;
                CheckSelection();
            }
            clickCount = 0;
        }
     
        
        private void CheckSelection() 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            var selection = hit.transform.GetComponent<ISelection>();

            if (selection == null) return;
            
            switch (selection)
            {
                case EmployeesBase target:
                    _target = target;
                    if(!TryVisibilityEmployees()) return;
                    
                    _signalBus.Fire(new SelectTargetSignal(_target));
                    break;
                
                default:
                    Debug.Log($"Fail in InputController");
                    return;
            }
                
            selection.ThisSelection(true);
            ResetTarget().Forget();
        }
        
        private bool TryVisibilityEmployees()
        {
            var position = _player.transform.position + new Vector3(0,0.5f,0);
            var direction = (_target.transform.position - position).normalized;
            
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out var raycastHit))
            {
                var employees = raycastHit.transform.GetComponent<EmployeesBase>();
                if (employees) return true;
            }
            
            _signalBus.Fire(new InfoSignal("employees", "Подойди ближе, в зону видимости"));
            return false;
        }
        
        private async UniTaskVoid ResetTarget() // todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
        {
            await UniTask.Delay(5000); // todo Мэджик
            _signalBus.Fire<LostTargetSignal>();
            
            if(_target == null) return;
            _target.ThisSelection(false);
            _target = null;
        }

      
    }
}