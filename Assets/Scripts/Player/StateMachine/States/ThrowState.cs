using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfases;
using Inventory;
using Employees;
using Enums;
using Pool;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Player
{
    public class ThrowState : IState, IInitializable, IDisposable
    {
        private EmployeesBase _target;
        private ThrowPoint _throwPoint;
        private TypeInventory _type;
        
        private float _speedRotate = 7f;
        private bool _isLookAt;

        private Transform _transformRoom;
        private Vector3 _positionRoom;
        
        private readonly PlayerAnimator _animator;
        private readonly Player _player;
        private readonly SignalBus _signalBus;
        
        private readonly InventoryBase[] _items; // todo is to add to the pool
        
        private Pool<InventoryBase> _pool;

        public ThrowState(PlayerAnimator animator, Player player, SignalBus signalBus, params InventoryBase[] items)
        {
            _animator = animator;
            _player = player;
            _signalBus = signalBus;
            _items = items;
        }
        
        public void Initialize()
        {
            _throwPoint = _player.ThrowPoint;
            GeneratePool();
            
            _signalBus.Subscribe<SelectTargetSignal>(OnSelectedTarget);
            _signalBus.Subscribe<ScatterHereSignal>(OnScatterHere);
            
            _signalBus.Subscribe<ThrowStateSignal>(OnSelectedInventory);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<SelectTargetSignal>(OnSelectedTarget);
            _signalBus.Unsubscribe<ScatterHereSignal>(OnScatterHere);
            
            _signalBus.Unsubscribe<ThrowStateSignal>(OnSelectedInventory);
        }

        public async void Enter()
        {
            switch (_type)
            {
                case TypeInventory.Airplane:
                    await ThrowAirplane();
                    break;
                
                case TypeInventory.Files:
                    await ScatterFiles();
                    break;
                
                default:
                    await ThrowAt();
                    break;
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

            if (_pool.TryGetObject(out InventoryBase item, _type))
            {
                _isLookAt = true;
                item.transform.position = _throwPoint.transform.position;
                
                var transform = _target.transform;
                var point = transform.position + new Vector3(0,transform.localScale.y,0); 
                
                _animator.ThrowAtEmployees();
                await UniTask.Delay(2000);// todo Мэджик
                
                item.Used(true);
                item.Throw( point, new Vector3(360, 0, 360)).Forget();
                
                _isLookAt = false;
            }
        }

        private async UniTask ThrowAirplane()
        {
            if (_pool.TryGetObject(out InventoryBase air, _type))
            {
                var transform = air.transform;
                transform.position = _throwPoint.transform.position;
                
                var player = _player.transform;
                var direction = transform.position + player.forward.normalized * 15;
            
                var target = new Vector3(direction.x, 0.1f, direction.z);
                var rotation = new Vector3(0, player.rotation.y, 0);
                
                _animator.ThrowObject();
                await UniTask.Delay(900);// todo Мэджик
                
                air.Used(true);
                air.Throw(target, rotation).Forget();
            }
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
                
                var rotationY = Random.Range(0, 360);
                var randomDuration = Random.Range(0.2f, 1f);
                
                tasks.Add(files.Throw(randomVector,new Vector3(0, rotationY, 0),_transformRoom, randomDuration));
            }
            
            await UniTask.WhenAll(tasks);
            _player.OnIgnore(false);
        }

        private void OnSelectedTarget(SelectTargetSignal selectTarget)
        {
            _target = selectTarget.Target;
            Debug.Log(_target.name);
        }
        
        private void OnSelectedInventory(ThrowStateSignal inventory)
        {
            if(inventory.Type == TypeInventory.None) return;
            _type = inventory.Type;
        }
        
        private void OnScatterHere(ScatterHereSignal scatterHere)
        {
            _transformRoom = scatterHere.TransformRoom;
            _positionRoom = scatterHere.TransformRoom.position;
            _type = scatterHere.Type;
        }

        private void GeneratePool()
        {
            _pool = new Pool<InventoryBase>(_player.transform);
            foreach (var item in _items)
                _pool.GeneratePool(item, 2); // todo Мэджик !!!
        }
    }
}