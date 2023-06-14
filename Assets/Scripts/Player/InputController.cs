﻿using System;
using Cysharp.Threading.Tasks;
using Employees;
using Enums;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class InputController: ITickable
    {
        private EmployeesBase _target;
        
        private readonly SignalBus _signalBus;
        private readonly Player _player;

        public InputController(SignalBus signalBus, Player player)
        {
            _player = player;
            _signalBus = signalBus;
        }
        

        public void Tick()
        {
            if(_player.IsIgnore) return;
            
            if (Input.GetMouseButtonDown(1))  //  Input.touchCount > 1;
            {
                _signalBus.Fire(new TargetSelectedSignal(TypeInventory.Airplane));
                ResetTarget().Forget();// todo временно... нужно сделать проверку, если _target находиться в зоне видимости камеры то можно что-то делать
            }
            
            if (!Input.GetMouseButtonDown(0)) return;
            CheckSelection();
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
                    _signalBus.Fire(new TargetSelectedSignal(TypeInventory.Banana, _target));
                    break;
                
                default:
                    Debug.Log($"Fail in InputController");
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