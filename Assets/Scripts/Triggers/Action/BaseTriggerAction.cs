using Cysharp.Threading.Tasks;
using Player;
using Signals;
using UnityEngine;
using Zenject;

namespace Triggers.Action
{
    public abstract class BaseTriggerAction : MonoBehaviour
    {
        [Header("View Settings")]  
        [SerializeField] protected Sprite viewImage;
        [SerializeField] protected string nameTrigger = "Name";
        [SerializeField] protected string textInfo= "Info";
        [SerializeField] protected int durationProgress;
        
        [SerializeField] private CanvasGroup canvasGroup;
        
         private bool _isActiveTrigger;
        
        public string NameTrigger => nameTrigger;
        public bool IsActiveTrigger => _isActiveTrigger;
        
        protected SignalBus _signal;
        protected ProgressBar _progressBar;
        private Player.Player _player;
        
        [Inject]
        public void Construct(SignalBus signalBus, Player.Player player, ProgressBar progressBar)
        {
            _signal = signalBus;
            _player = player;
            _progressBar = progressBar;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isActiveTrigger) return;
            if (!other.GetComponent<Player.Player>() || _player.IsIgnore) return;
            
            _signal.Fire<ShowActionsSignal>();
            PlayerTriggerEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_isActiveTrigger) return;
            if (!other.GetComponent<Player.Player>()) return;
            
            _signal.Fire<LostTargetSignal>();
            PlayerTriggerExit();
        }
        
        public void TriggerActive(bool isOn)
        {
            if (isOn)
            {
                canvasGroup.alpha = 1;
                _isActiveTrigger = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                _isActiveTrigger = false;
            }
        }
        
        protected abstract void PlayerTriggerEnter();
        protected abstract void PlayerTriggerExit();

        public abstract UniTask Break();
        public abstract UniTask Change();
        public abstract void PickUp(Transform parentTransform);
        
    }
}