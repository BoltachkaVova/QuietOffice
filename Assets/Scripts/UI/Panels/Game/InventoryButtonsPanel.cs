using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Signals;
using UI.Buttons.Actions;
using UI.Buttons.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Panels.Game
{
    public class InventoryButtonsPanel : MonoBehaviour
    {
        private Image _image;

        private Dictionary<TypeInventory, SelectInventoryButton> _dictionaryButtons;
        
        private SignalBus _signal;
        private Player.Player _player;
        
        [Inject]
        public void Construct(SignalBus signalBus, Player.Player player)
        {
            _signal = signalBus;
            _player = player;
        }

        private void Awake()
        {
            _dictionaryButtons = GetComponentsInChildren<SelectInventoryButton>(true)
                .ToDictionary(button => button.TypeButtonInventory, button => button);
            
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _signal.Subscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Subscribe<LostTargetSignal>(OnTargetLost);
            
            _signal.Subscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Unsubscribe<LostTargetSignal>(OnTargetLost);
            
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnSelectTarget(SelectTargetSignal target)
        {
            ShowButtons();
        }

        private void OnThrow()
        {
            CloseButtons();
        }
        
        private void OnTargetLost()
        {
            CloseButtons();
        }

        private void ShowButtons()
        {
            bool isEat = false;
            var dictionary = _player.Inventory;
            
            foreach (var item in dictionary.Where(item => item.Value > 0))
            {
                isEat = true;
                _dictionaryButtons[item.Key].SetCount(item.Value);
                _dictionaryButtons[item.Key].gameObject.SetActive(true);
            }
            
            if(isEat)
                 _image.DOFade(0.4f , 0.2f); // todo Мэджик и ниже тоже 
            else
                 _signal.Fire(new InfoSignal("Inventory", "Карматы твои пусты!!! найди мусорку, там что-то точно должно быть"));
        }

        private void CloseButtons()
        {
            foreach (var button in _dictionaryButtons)
                button.Value.gameObject.SetActive(false);
            
            _image.DOFade( 0, 0.2f); // todo Мэджик
        }
    }
}