﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _viewImage;
        
        private CanvasGroup _canvasGroup;
        private Image _bar;
        
        private Transform _cameraTransform;
        private Coroutine _progressCoroutine;
        
        private bool _isActive;
        private bool _isDone;
        private float _durationProgress;
        
        public bool IsActive => _isActive;
        public bool IsDone => _isDone;

        private void Awake()
        {
            _bar = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _bar.fillAmount = 0;
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if(!_isActive) return;
            transform.rotation = Quaternion.LookRotation(_cameraTransform.forward);
        }

        public void Show(float durationProgress, Sprite viewImage) 
        {
            _isActive = true;
            _isDone = false;

            _viewImage.sprite = viewImage;
            _canvasGroup.alpha = 1;
            _durationProgress = durationProgress;
            
            _progressCoroutine = StartCoroutine(FillProgress());
        }

        public void Close(bool isDone)
        {
            _isActive = false;
            _isDone = isDone;
            
            if(_progressCoroutine != null)
               StopCoroutine(_progressCoroutine);
            
            _canvasGroup.alpha = 0;
            _bar.fillAmount = 0;
        }

        private IEnumerator FillProgress()
        {
            var elapsedTime = 0f;
            var startFillAmount = 0;
            var targetFillAmount = 1f;

            while (elapsedTime < _durationProgress)
            {
                elapsedTime += Time.deltaTime;
                _bar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / _durationProgress);
                yield return null;
            }
            
            _bar.fillAmount = targetFillAmount;
            Close(true);
        }
    }
}