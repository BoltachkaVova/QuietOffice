using Player;
using UnityEngine;
using Zenject;

namespace Triggers.Perform
{
    public abstract class TriggerPerform :TriggerBase
    {
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
            
            if (other.GetComponent<Player.Player>())
                PlayerTriggerEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>())
            {
               _progressBar.Close(false);
                PlayerTriggerExit();
            }
        }

        protected abstract void PlayerTriggerEnter();
        protected abstract void PlayerTriggerExit();
    }
}