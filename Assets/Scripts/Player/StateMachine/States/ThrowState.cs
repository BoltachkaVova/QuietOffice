using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfases;
using Inventory;
using Employees;
using Enums;
using Room;
using Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Player
{
    public class ThrowState : IState, IInitializable, IDisposable
    {
        private EmployeesBase _target;
        private ThrowPoint _throwPoint;
        private TypeInventory _typeInventory;
        
        private float _speedRotate = 7f;
        private bool _isLookAt;

        private Transform _transformRoom;
        private Vector3 _positionRoom;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signalBus;
        private readonly InventoryBase banana;
        private readonly InventoryBase airplane;

        public ThrowState(PlayerAnimator animator, Player player, SignalBus signalBus, InventoryBase banana, InventoryBase airplane)
        {
            _animator = animator;
            _player = player;
            _signalBus = signalBus;
            this.banana = banana;
            this.airplane = airplane;
        }
        
        public void Initialize()
        {
            _throwPoint = _player.ThrowPoint;
            
            _signalBus.Subscribe<TargetSelectedSignal>(OnTargetSelected);
            _signalBus.Subscribe<BusySignal>(OnBusy);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<TargetSelectedSignal>(OnTargetSelected);
            _signalBus.Unsubscribe<BusySignal>(OnBusy);
        }

        public async void Enter()
        {
            switch (_typeInventory)
            {
                case TypeInventory.Airplane:
                    await Throw();
                    break;
                
                case TypeInventory.Banana:
                    await ThrowAt();
                    break;
                
                case TypeInventory.Files:
                    await Scatter();
                    break;
                
                case TypeInventory.None:
                    Debug.Log(TypeInventory.None);
                    break;
            }
            
        }

        public void Update()
        {
            if (!_isLookAt)  return;
            LookAt();
        }

        public void Exit()
        {
           
        }

        private void LookAt()
        {
            var transform = _player.transform;
            var direction = (_target.transform.position - transform.position).normalized;
            
            _player.transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(direction),
                _speedRotate * Time.deltaTime);
        }

        private async UniTask ThrowAt()
        {
            _isLookAt = true;
            
            _animator.ThrowAtEmployees();
            await UniTask.Delay(2000);// todo Мэджик
            
            var banan = Object.Instantiate(banana, _throwPoint.transform.position, Quaternion.identity); // todo временно!! 
            await banan.Throw(null, _target.transform.position, new Vector3(360, 0, 360), 1f);
            _isLookAt = false;
            
            ReturnActiveState();
        }

        private async UniTask Throw()
        {
            _animator.ThrowObject();
            await UniTask.Delay(900);// todo Мэджик
            
            var transform = _throwPoint.transform;
            Object.Instantiate(airplane, transform.position, transform.rotation);
            
            ReturnActiveState();
        }
        
        private async UniTask Scatter()
        {
            List<UniTask> tasks = new List<UniTask>(20);
            
            var fileses = _player.OfficeFileses; // todo this Pool
            var localScale = _transformRoom.localScale;
            
            foreach (var files in fileses)
            {
                var randomVector = new Vector3(Random.Range(_positionRoom.x, _positionRoom.x + localScale.x),
                    _positionRoom.y,Random.Range(_positionRoom.z, _positionRoom.z + localScale.z));
                var rotationX = Random.Range(0, 360);
                var randomDuration = Random.Range(0.2f, 1f);
                
                tasks.Add(files.Throw(_transformRoom, randomVector, new Vector3(rotationX, -90, 0), randomDuration));
            }
            await UniTask.WhenAll(tasks);
            ReturnActiveState();
        }

        private void OnTargetSelected(TargetSelectedSignal selected)
        {
            _target = selected.Target;
            _typeInventory = selected.TypeInventory;
        }
        
        private void OnBusy(BusySignal busySignal)
        {
            _transformRoom = busySignal.TransformRoom;
            _positionRoom = busySignal.TransformRoom.position;
        }


        private void ReturnActiveState()
        {
            _signalBus.Fire<ActiveStateSignal>();
        }
        
    }
}