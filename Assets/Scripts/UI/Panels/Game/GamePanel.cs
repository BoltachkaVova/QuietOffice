using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;


namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        private TakeButton _takeButton;
        private ThrowButton _throwButton;
        private StopWorkButton _stopWorkButton;
        private AttackButton _attackButton;
        private ExitAttackButton _exitAttackButton;

        private SignalBus _signal;
        private InformationPanel _informationPanel;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signal = signalBus;
        }

        private void Awake()
        {
            _throwButton = GetComponentInChildren<ThrowButton>(true);
            _takeButton = GetComponentInChildren<TakeButton>(true);
            _stopWorkButton = GetComponentInChildren<StopWorkButton>(true);
            _attackButton = GetComponentInChildren<AttackButton>(true);
            _exitAttackButton = GetComponentInChildren<ExitAttackButton>(true);
        }

        private void Start()
        {
            _signal.Subscribe<WorkSignal>(OnShowStopWorkButton);
            _signal.Subscribe<StopWorkSignal>(OnStopWork);
            
            _signal.Subscribe<TargetSelectedSignal>(OnTargetSelected);
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
 
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<WorkSignal>(OnShowStopWorkButton);  // тут все норм(наверн нужно просто рекламу засунуть да и все ненужна тут кнопка StopWorkSignal)
            _signal.Unsubscribe<StopWorkSignal>(OnStopWork);
            
            _signal.Unsubscribe<TargetSelectedSignal>(OnTargetSelected);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);

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
    }
}