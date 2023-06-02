using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ProgressBar : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Transform _cameraTransform;
        
        private Image _viewImage;
        private Image _bar;
        
        private bool _isActive;

        public bool IsActive => _isActive;

        private void Awake()
        {
            _viewImage = GetComponentInChildren<ViewImage>().GetComponent<Image>();
            _bar = GetComponent<Image>();
            _bar.fillAmount = 0;
            
            _canvasGroup = GetComponent<CanvasGroup>();
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if(!_isActive) return;
            transform.rotation = Quaternion.LookRotation(_cameraTransform.forward);
        }

        public async void Show(float durationProgress, Sprite viewImage) // todo сделано так чтоб просто работало... 
        {
            _bar.fillAmount = 0;
            _isActive = true;
            _viewImage.sprite = viewImage;
            _canvasGroup.alpha = 1;

            await _bar.DOFillAmount(1, durationProgress).SetEase(Ease.Linear);
            _canvasGroup.alpha = 0;
        }

        public void Close()
        {
            _bar.fillAmount = 0;
            _canvasGroup.alpha = 0;
            _isActive = false;
        }
    }
}