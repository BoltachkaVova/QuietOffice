using System;
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

        [SerializeField] private CanvasGroup canvasGroup;
        
        private bool _isActiveTrigger;
        
        protected SignalBus _signal;
        protected Player.Player _player;
        protected ProgressBar _progressBar;
        public bool IsActiveTrigger => _isActiveTrigger;

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
            if (!other.GetComponent<Player.Player>()) return;
            
            PlayerTriggerEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<Player.Player>()) return;
            
            _progressBar.Close(false);
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
    }
}