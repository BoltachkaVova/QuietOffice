using Player;
using UnityEngine;
using Zenject;

namespace Triggers.Perform
{
    public abstract class BaseTriggerPerform : MonoBehaviour
    {
        [Header("View Settings")]  
        [SerializeField] protected Sprite viewImage;
        [SerializeField] protected string nameTrigger = "Name";
        [SerializeField] protected string textInfo= "Info";
        [SerializeField] protected int durationProgress;
        [SerializeField] protected bool isActiveTrigger;
        
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
            if(!isActiveTrigger) return; // todo будет упровляться "левелом" т.е левел будет вкл/выкл триггеры
            if (!other.GetComponent<Player.Player>()) return;
            
            PlayerTriggerEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<Player.Player>()) return;
            
            _progressBar.Close(false);
            PlayerTriggerExit();
        }

        protected abstract void PlayerTriggerEnter();
        protected abstract void PlayerTriggerExit();
    }
}