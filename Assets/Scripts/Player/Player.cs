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
        
        public ThrowPoint ThrowPoint => _throwPoint;
        public bool IsIgnore => _isIgnore;
        public List<OfficeFiles> OfficeFileses => _officeFileses;
        

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
        

        private async void OnTriggerEnter(Collider other) // todo в Level просто вкл/выкл триггеры 
        {
            if (other.TryGetComponent(out TriggerWaitingBase triggerWaiting))
            {
                if(!triggerWaiting.IsActive || _isIgnore) return;
                
                _progressBar.Show(triggerWaiting.DurationProgress, triggerWaiting.ViewImage);
                await UniTask.WaitUntil(() => !_progressBar.IsActive);
                
                if(!_progressBar.IsDone) return;
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
            
            if(other.GetComponent<Scatter>())
                _signalBus.Fire<TargetLostSignal>();
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
                    await UniTask.WaitUntil(() => !printer.IsActive);
                    _signalBus.Fire<ActiveStateSignal>();
                    break;
                
                case TrashBin trashBin:
                    Debug.Log($"Подобрали {trashBin.Inventory.Count}  мусора");
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
                    _signalBus.Fire(new ScatterHereSignal(trigger.transform)); // todo мб сделать один сигнал? 
                    _signalBus.Fire(new SelectTargetSignal(null,TypeInventory.OffiseFiles));
                    _signalBus.Subscribe<ThrowStateSignal>(OnThrowSignal);
                    break;
                
                default:
                    Debug.Log($"Fail in Player {trigger.name}");
                    return;
            }
        }
        
        private void OnThrowSignal()
        {
            _isIgnore = false;
            _officeFileses.Clear();
            
            _signalBus.Unsubscribe<ThrowStateSignal>(OnThrowSignal);
        }

    }
}