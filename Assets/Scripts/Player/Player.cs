using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enums;
using Inventory;
using Signals;
using Room;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform transformPoint;

        private ThrowPoint _throwPoint;
        private ProgressBar _progressBar;
        private SignalBus _signalBus;

        private bool _isIgnore = false;
        private bool _isDone = false;
        
        private List<OfficeFiles> _officeFileses = new List<OfficeFiles>(20);
        private Dictionary<TypeInventory, int> _inventory = new Dictionary<TypeInventory, int>(10);

        public ThrowPoint ThrowPoint => _throwPoint;
        
        public bool IsIgnore => _isIgnore;
        public bool IsDone => _isDone;
        
        public List<OfficeFiles> OfficeFileses => _officeFileses;
        public Dictionary<TypeInventory, int> Inventory => _inventory;
        public Transform TransformPoint => transformPoint;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _throwPoint = GetComponentInChildren<ThrowPoint>();
            _progressBar = GetComponentInChildren<ProgressBar>();
        }

        private void Start()
        {
            _signalBus.Subscribe<ThrowStateSignal>(RemoveInventory);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Printer>())
                _officeFileses = GetComponentsInChildren<OfficeFiles>().ToList(); // todo временно
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<ThrowStateSignal>(RemoveInventory);
        }
        
        public async UniTask ShowProgress(float duration, Sprite sprite)
        {
            _progressBar.Show(duration, sprite);
            await UniTask.WaitUntil(() => !_progressBar.IsActive);
            
            if (!_progressBar.IsDone) return;
            _isDone = true;
        }

        public void CloseProgress()
        {
            _isDone = false;
            _progressBar.Close(false);
        }
        
        public void AddInventory(int count, TypeInventory typeInventory)
        {
            if (_inventory.ContainsKey(typeInventory))
                _inventory[typeInventory] += count;
            else
                _inventory.Add(typeInventory, count);
        }
        
        private void RemoveInventory(ThrowStateSignal key)
        {
            if (!_inventory.ContainsKey(key.Type)) return;
            _inventory[key.Type] --;
        }

        public void OnIgnore(bool isOn)
        {
            _isIgnore = isOn;

            if (!_isIgnore)
                _officeFileses.Clear();
        }
    }
}