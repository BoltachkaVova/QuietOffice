using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Signals;
using Signals.Trigger;
using Triggers.Action;
using Triggers.Perform;
using UnityEngine;
using Zenject;

namespace QuietOffice.Triggers
{
    public class TriggerController : MonoBehaviour
    {
        [Header("SettingsTriggerController")] [SerializeField]
        private float timeStart;

        [SerializeField] private float timeResetFlyingPaper;
        [SerializeField] private float timeResetTrashBin;
        [SerializeField] private float timeResetPrinter;
        [SerializeField] private float timeResetScatter;


        [Header("Triggers")] 
        [SerializeField] private FlyingPaper[] flyingPaper;
        [SerializeField] private Scatter[] scatters;
        [SerializeField] private TrashBin[] trashBins;
        [SerializeField] private Printer[] printers;

        private bool _isActiveFly = false;
        private bool _isActiveTrashBin;
        private bool _isActive = false;
        private float _counter;

        private SignalBus _signal;

        [Inject]
        public void Construct(SignalBus signal)
        {
            _signal = signal;
        }

        private void Start()
        {
            _signal.Subscribe<FlyingSignal>(OnFlyPaper);
            _signal.Subscribe<TrashBinSignal>(OnTrashBin);
            _signal.Subscribe<ScatterSignal>(OnScatter);
        }


        private void Update()
        {
            if (!_isActive) return;
            _counter += Time.deltaTime;

            if (!(_counter >= timeStart)) return;
            
            if (!_isActiveTrashBin)
                OpenTrashBin().Forget();

            _counter = 0;
        }

        private void OnDestroy()
        {
            _signal.Unsubscribe<FlyingSignal>(OnFlyPaper);
            _signal.Unsubscribe<TrashBinSignal>(OnTrashBin);
            _signal.Unsubscribe<ScatterSignal>(OnScatter);
        }

        private void OnFlyPaper()
        {

        }

        private void OnTrashBin()
        {

        }

        private async void OnScatter()
        {
            var scatter = scatters.FirstOrDefault(o => !o.IsActiveTrigger);

            scatter.TriggerActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(timeResetScatter));
            scatter.TriggerActive(false);
        }

        private async UniTaskVoid OpenTrashBin()
        {
            _isActiveTrashBin = true;
            var trashBin = trashBins.FirstOrDefault(o => !o.IsActiveTrigger);
            trashBin.TriggerActive(true);
        }

    }
}