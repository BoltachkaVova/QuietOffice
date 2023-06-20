using System;
using UnityEngine;
using Zenject;

namespace Room
{
    public abstract class TriggerPerformBase : MonoBehaviour
    {
        protected SignalBus _signal;
        protected Player.Player _player;

        [Inject]
        public void Construct(SignalBus signalBus, Player.Player player)
        {
            _signal = signalBus;
            _player = player;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Player.Player>() && _player.IsIgnore)
                PlayerTriggerEnter();
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