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
        
        private List<OfficeFiles> _officeFileses = new List<OfficeFiles>(20);
        private Dictionary<TypeInventory, int> _inventory = new Dictionary<TypeInventory, int>(10);

        public ThrowPoint ThrowPoint => _throwPoint;
        public bool IsIgnore => _isIgnore;
        public List<OfficeFiles> OfficeFileses => _officeFileses;
        public Dictionary<TypeInventory, int> Inventory => _inventory;

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
        
        private async void OnTriggerEnter(Collider other) // todo в Level просто вкл/выкл триггеры 
        {
            if (other.TryGetComponent(out TriggerWaitingBase triggerWaiting))
            {
                if (!triggerWaiting.IsActiveTrigger || _isIgnore) return;

                _progressBar.Show(triggerWaiting.DurationProgress, triggerWaiting.ViewImage);
                await UniTask.WaitUntil(() => !_progressBar.IsActive);

                if (!_progressBar.IsDone) return;
                CheckTriggerWaiting(triggerWaiting);
            }

            if (other.TryGetComponent(out TriggerPerformBase triggerPerform) && _isIgnore)
                CheckTriggerPerform(triggerPerform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerWaitingBase>())
                _progressBar.Close(false);

            if (other.GetComponent<Printer>())
                _officeFileses = GetComponentsInChildren<OfficeFiles>().ToList(); // todo временно

            if (other.GetComponent<Scatter>())
                _signalBus.Fire<TargetLostSignal>();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<ThrowStateSignal>(RemoveInventory);
        }

        private async void CheckTriggerWaiting(TriggerWaitingBase component)
        {
            switch (component)
            {
                case Work workTrigger:
                    _signalBus.Fire(new WorkStateSignal(workTrigger.transform, workTrigger.Chair.transform));
                    break;

                case Printer printer:
                    _isIgnore = true;
                    _signalBus.Fire<IdleStateSignal>();
                    printer.PickUp(transformPoint);
                    await UniTask.WaitUntil(() => !printer.IsActiveTrigger);
                    _signalBus.Fire<ActiveStateSignal>();
                    break;

                case TrashBin trashBin:
                    foreach (var trash in trashBin.TrashBins)
                        AddInventory(trash.Count, trash.Inventory);
                    break;

                default:
                    Debug.Log($"Fail in Player {component.name}");
                    return;
            }
            _signalBus.Fire(new InfoInventorySignal(component.NameTrigger, component.TextInfo));
        }
        
        private void CheckTriggerPerform(TriggerPerformBase trigger)
        {
            switch (trigger)
            {
                case Scatter:
                    _signalBus.Fire(new ScatterHereSignal(trigger.transform, TypeInventory.Files));
                    break;

                default:
                    Debug.Log($"Fail in Player {trigger.name}");
                    return;
            }
        }
        
        private void RemoveInventory(ThrowStateSignal key)
        {
            if (!_inventory.ContainsKey(key.Type)) return;
            _inventory[key.Type] --;
        }
        
        private void AddInventory(int count, TypeInventory typeInventory)
        {
            if (_inventory.ContainsKey(typeInventory))
                _inventory[typeInventory] += count;
            else
                _inventory.Add(typeInventory, count);
        }

        public void OnIgnore(bool isOn)
        {
            _isIgnore = isOn;

            if (!_isIgnore)
                _officeFileses.Clear();
        }
    }
}