using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WorkObjects
{
    public class Printer : MonoBehaviour
    {
        [SerializeField] private float timeSpawnFiles = 1f;
        
        
        [SerializeField] private int countSpawnFiles;
        [SerializeField] private OfficeFiles prefabFiles;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform endPoint;
        private Vector3 _endPoint;
        
        [SerializeField] private Transform printerView;
        
        [SerializeField] private float shakeForce = 0.05f; 
        [SerializeField] private float shakeRandomness = 90f; 
        [SerializeField] private int shakeVibrato = 10; 
        
        [SerializeField] private Vector3 scaleChangeAmount = new Vector3(0.2f, 0.2f, 0f);

        private Vector3 _startPrinterScale;
        private Sequence printerSequence;
        
        private float shakeDuration;  
        private float scaleChangeDuration;
        
        private List<OfficeFiles> _officeFileses =  new List<OfficeFiles>(10);

        private async void Start()
        {
            _endPoint = endPoint.position;
            
            _startPrinterScale = printerView.localScale;
            shakeDuration = scaleChangeDuration = timeSpawnFiles * 0.5f;

            for (int i = 0; i < countSpawnFiles; i++)
            {
               await StartPrinting();
            }
        }

        private void OnDestroy()
        {
            printerSequence.Kill();
        }

        private async UniTask StartPrinting()
        {
            printerSequence = DOTween.Sequence();
            await printerSequence.Append(printerView.DOShakePosition(shakeDuration, shakeForce, shakeVibrato, shakeRandomness))
                .Join(printerView.DOScale(_startPrinterScale + scaleChangeAmount, scaleChangeDuration))
                .Append(printerView.DOScale(_startPrinterScale, scaleChangeDuration))
                .OnComplete(ResetPrinter);
        }

        private void ResetPrinter()
        {
            // todo сделать пулл
            
            printerView.localScale = _startPrinterScale;
            
            var files = Instantiate(prefabFiles, spawnPoint.position, Quaternion.Euler(90,90, 0));
            _officeFileses.Add(files);
            
            files.transform.DOJump(_endPoint, 2f, 1, 1f).SetEase(Ease.InOutSine)
                .Join(files.transform.DORotate(new Vector3(0,90,0),1.5f)).SetEase(Ease.Linear);

            _endPoint.y += files.transform.localScale.y * 0.015f; // todo мэджик 0.015f... фрефаб "гг"
        }

    }
}