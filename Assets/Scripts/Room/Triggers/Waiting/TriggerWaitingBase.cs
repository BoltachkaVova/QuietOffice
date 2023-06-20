using UnityEngine;
using Zenject;

namespace Room
{
    public abstract class TriggerWaitingBase : MonoBehaviour
    {
        [Header("View Settings")]  
        [SerializeField] protected Sprite viewImage;
        [SerializeField] protected string nameTrigger = "Name";
        [SerializeField] protected string textInfo= "Info";
        [SerializeField] protected int durationProgress;
        
        [SerializeField] protected bool isActiveTrigger;

        protected SignalBus _signal;
        protected Player.Player _player;
        
        public bool IsActiveTrigger => isActiveTrigger;

        [Inject]
        public void Construct(SignalBus signalBus, Player.Player player)
        {
            _signal = signalBus;
            _player = player;
        }

        private async void OnTriggerEnter(Collider other)
        {
            if(!isActiveTrigger) return;
            
            if (other.GetComponent<Player.Player>() && !_player.IsIgnore)
            {
                await _player.ShowProgress(durationProgress, viewImage);
                if(!_player.IsDone) return;
                PlayerTriggerEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.GetComponent<Player.Player>())
                PlayerTriggerExit();
        }
        
        protected abstract void PlayerTriggerEnter();
        protected abstract void PlayerTriggerExit();
    }
}