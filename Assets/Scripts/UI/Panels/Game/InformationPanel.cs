using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class InformationPanel : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private TextMeshProUGUI _infoText;
        private TextMeshProUGUI _nameText;
        
        
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            
            _infoText = GetComponentInChildren<InfoText>().GetComponent<TextMeshProUGUI>();
            _nameText = GetComponentInChildren<NameText>().GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _signalBus.Subscribe<InfoInventorySignal>(OnShowInfoPanel);
        }
        
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<InfoInventorySignal>(OnShowInfoPanel);
        }
        
        private async void OnShowInfoPanel(InfoInventorySignal obj)
        {
            _infoText.text = obj.InfoText;
            _nameText.text = obj.NameText;
            await _canvasGroup.DOFade(1, 3f); // todo мЭджик 3f
            await _canvasGroup.DOFade(0, 3f);
        }
    }
}