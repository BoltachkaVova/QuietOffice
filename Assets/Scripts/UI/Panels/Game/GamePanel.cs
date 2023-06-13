using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;


namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private ThrowButton _throwButton;
        private StopWorkButton _stopWorkButton;

        private SignalBus _signal;
        private InformationPanel _informationPanel; // todo entity 

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
        }

        private void Awake()
        {
            _throwButton = GetComponentInChildren<ThrowButton>(true);
            _stopWorkButton = GetComponentInChildren<StopWorkButton>(true);
        }

        private void Start()
        {
            _signal.Subscribe<WorkStateSignal>(OnShowStopWorkButton);
            _signal.Subscribe<ActiveStateSignal>(OnStopWork);
            
            _signal.Subscribe<TargetSelectedSignal>(OnTargetSelected);
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
            
            
            _signal.Subscribe<BusySignal>(OnBusy);
        }
        

        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkStateSignal>(OnShowStopWorkButton);  // тут все норм(наверн нужно просто рекламу засунуть да и все ненужна тут кнопка StopWorkSignal)
            _signal.Unsubscribe<ActiveStateSignal>(OnStopWork);
            
            _signal.Unsubscribe<TargetSelectedSignal>(OnTargetSelected);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);
            
            
            _signal.Unsubscribe<BusySignal>(OnBusy);
        }
        
        private async void OnShowStopWorkButton()
        {
            await UniTask.Delay(10000);
            _stopWorkButton.gameObject.SetActive(true);
        }
        
        private void OnStopWork()
        {
            _stopWorkButton.gameObject.SetActive(false);
        }
        
        private void OnTargetSelected()
        {
            // todo тут вывести(панель), что можно сделать с этой целью, а пока что просто кнопка бросить "банан"
            
            _throwButton.gameObject.SetActive(true);
        }
        
        private void OnTargetLost()
        {
            _throwButton.gameObject.SetActive(false);
        }
        
        private void OnBusy()
        {
            _throwButton.gameObject.SetActive(true);
        }
    }
}