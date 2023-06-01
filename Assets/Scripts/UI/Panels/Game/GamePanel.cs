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
        }

        private void Start()
        {
            _signal.Subscribe<TakeSignal>(OnShowAttackButton);
            _signal.Subscribe<WorkSignal>(OnShowStopWorkButton);
            _signal.Subscribe<StopWorkSignal>(OnStopWork);
            _signal.Subscribe<AttackSignal>(OnShowThrowButton);
            _signal.Subscribe<ThrowSignal>(OnShowButtons);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<TakeSignal>(OnShowAttackButton);
            _signal.Unsubscribe<WorkSignal>(OnShowStopWorkButton);
            _signal.Unsubscribe<StopWorkSignal>(OnStopWork);
            _signal.Unsubscribe<AttackSignal>(OnShowThrowButton);
            _signal.Unsubscribe<ThrowSignal>(OnShowButtons);
        }
        
        private void OnShowButtons()
        {
            _throwButton.gameObject.SetActive(false); 
            _attackButton.gameObject.SetActive(false);
        }


        private void OnShowAttackButton()
        {
            _attackButton.gameObject.SetActive(true);
            _throwButton.gameObject.SetActive(false);
        }
        
        private void OnShowThrowButton()
        {
            _throwButton.gameObject.SetActive(true); 
            _attackButton.gameObject.SetActive(false);
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
    }
}