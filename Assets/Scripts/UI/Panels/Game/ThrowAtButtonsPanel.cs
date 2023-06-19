﻿using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Signals;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ThrowAtButtonsPanel : MonoBehaviour
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
            _signal.Subscribe<TargetLostSignal>(OnTargetLost);
            
            _signal.Subscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnDestroy()
        {
            _signal.Unsubscribe<SelectTargetSignal>(OnSelectTarget);
            _signal.Unsubscribe<TargetLostSignal>(OnTargetLost);
            
            _signal.Unsubscribe<ThrowStateSignal>(OnThrow);
        }
        
        private void OnSelectTarget(SelectTargetSignal target)
        {
            // todo добавить проверку на таргет/ что можно с ним делать? 
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
            
            foreach (var item in dictionary)
            {
                if (item.Value <= 0) continue;
                
                isEat = true;
                _dictionaryButtons[item.Key].SetCount(item.Value);
                _dictionaryButtons[item.Key].gameObject.SetActive(true);
            }
            
            if(isEat)
                 _image.DOFade(0.4f , 0.2f); // todo Мэджик
            else
                 _signal.Fire(new InfoInventorySignal("Inventory", "Карматы твои пусты!!! найди мусорку, там что-то точно должно быть"));
        }

        private void CloseButtons()
        {
            foreach (var button in _dictionaryButtons)
                button.Value.gameObject.SetActive(false);
            
            _image.DOFade( 0, 0.2f); // todo Мэджик
        }
    }
}