﻿using System;
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
        
        private readonly InventoryBase _banana; // todo is to add to the pool
        private readonly InventoryBase _airplane; // todo is to add to the pool

        public ThrowState(PlayerAnimator animator, Player player, SignalBus signalBus, InventoryBase banana, InventoryBase airplane)
        {
            _animator = animator;
            _player = player;
            _signalBus = signalBus;
            _banana = banana;
            _airplane = airplane;
        }
        
        public void Initialize()
        {
            _throwPoint = _player.ThrowPoint;
            
            _signalBus.Subscribe<SelectedSignal>(OnTargetSelected);
            _signalBus.Subscribe<ScatterHereSignal>(OnScatterHere);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<SelectedSignal>(OnTargetSelected);
            _signalBus.Unsubscribe<ScatterHereSignal>(OnScatterHere);
        }

        public async void Enter()
        {
            switch (_typeInventory)
            {
                case TypeInventory.Airplane:
                    await ThrowAirplane();
                    break;

                case TypeInventory.Banana:
                    await ThrowAt();
                    break;

                case TypeInventory.Files:
                    await ScatterFiles();
                    break;

                case TypeInventory.None:
                    Debug.Log(TypeInventory.None);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _signalBus.Fire<ActiveStateSignal>();
        }

        public void Update()
        {
            if (!_isLookAt)  return;
            LookAt();
        }

        public void Exit()
        {
            _signalBus.Fire<TargetLostSignal>();
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
            
            var banan = Object.Instantiate(_banana, _throwPoint.transform.position, Quaternion.identity); // todo in pool
            await banan.Throw( _target.transform.position, new Vector3(360, 0, 360));
            _isLookAt = false;
        }

        private async UniTask ThrowAirplane()
        {
            _animator.ThrowObject();
            await UniTask.Delay(900);// todo Мэджик
            
            var transform = _throwPoint.transform;
            var air = Object.Instantiate(_airplane, transform.position, transform.rotation); // todo in pool

            var player = _player.transform;
            var direction = air.transform.position + player.forward.normalized * 15;
            
            var target = new Vector3(direction.x, 0.1f, direction.z);
            var rotation = new Vector3(0, player.rotation.y, 0);
            
            air.Throw(target, rotation);
        }
        
        private async UniTask ScatterFiles()
        {
            List<UniTask> tasks = new List<UniTask>(20);
            
            var fileses = _player.OfficeFileses; 
            var localScale = _transformRoom.localScale;
            
            foreach (var files in fileses)
            {
                var randomVector = new Vector3(Random.Range(_positionRoom.x, _positionRoom.x + localScale.x),
                    _positionRoom.y,Random.Range(_positionRoom.z, _positionRoom.z + localScale.z));
                
                var rotationX = Random.Range(0, 360);
                var randomDuration = Random.Range(0.2f, 1f);
                
                tasks.Add(files.Throw(randomVector,new Vector3(rotationX, -90, 0), _transformRoom, randomDuration));
            }
            
            await UniTask.WhenAll(tasks);
        }

        private void OnTargetSelected(SelectedSignal selected)
        {
            _target = selected.Target;
            _typeInventory = selected.TypeInventory;
        }
        
        private void OnScatterHere(ScatterHereSignal scatterHereSignal)
        {
            _transformRoom = scatterHereSignal.TransformRoom;
            _positionRoom = scatterHereSignal.TransformRoom.position;
        }
    }
}