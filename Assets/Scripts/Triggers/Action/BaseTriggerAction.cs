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
        [SerializeField] protected bool isActiveTrigger;
        
        public string NameTrigger => nameTrigger;
        public bool IsActiveTrigger => isActiveTrigger;
       
        
        protected SignalBus _signal;
        protected Player.Player _player;
        protected ProgressBar _progressBar;
        
        [Inject]
        public void Construct(SignalBus signalBus, Player.Player player, ProgressBar progressBar)
        {
            _signal = signalBus;
            _player = player;
            _progressBar = progressBar;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isActiveTrigger) return;
            if (!other.GetComponent<Player.Player>() || _player.IsIgnore) return;
            
            _signal.Fire<ShowActionsSignal>();
            PlayerTriggerEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if(!isActiveTrigger) return;
            if (!other.GetComponent<Player.Player>()) return;
            
            _signal.Fire<LostTargetSignal>();
            PlayerTriggerExit();
        }
        
        protected abstract void PlayerTriggerEnter();
        protected abstract void PlayerTriggerExit();
        
        public abstract UniTask Break();
        public abstract UniTask Change();
        public abstract void PickUp(Transform parentTransform);
    }
}