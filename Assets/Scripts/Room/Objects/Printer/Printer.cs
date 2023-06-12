using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Room
{
    public class Printer : MonoBehaviour
    {
        [Header("")]
        [SerializeField] private OfficeFiles prefabFiles;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform printerView;
        
        [Header("Settings Animation")]
        [SerializeField] private int countSpawnFiles = 30;
        [SerializeField] private float timeSpawnFiles = 1f;
        [SerializeField] private float shakeForce = 0.05f; 
        [SerializeField] private float shakeRandomness = 90f; 
        [SerializeField] private int shakeVibrato = 10;
        [SerializeField] private Vector3 scaleChangeAmount = new Vector3(0.2f, 0.2f, 0f);

        private Vector3 _startPrinterScale;
        private Vector3 _endPoint;
        
        private Sequence printerSequence;
        
        private float shakeDuration;  
        private float scaleChangeDuration;

        private bool _isDone;
        
        private Stack<OfficeFiles> _officeFileses =  new Stack<OfficeFiles>(10);

        public bool IsDone => _isDone;

        private async void Start()
        {
            _startPrinterScale = printerView.localScale;
            shakeDuration = scaleChangeDuration = timeSpawnFiles * 0.5f;

            await StartPrinting();
        }

        private void OnDestroy()
        {
            printerSequence.Kill();
        }

        private async UniTask StartPrinting()
        {
            _isDone = false;
            _endPoint = endPoint.position;
            
            for (int i = 0; i < countSpawnFiles; i++)
            {
                printerSequence = DOTween.Sequence();
                await printerSequence.Append(printerView.DOShakePosition(shakeDuration, shakeForce, shakeVibrato, shakeRandomness))
                    .Join(printerView.DOScale(_startPrinterScale + scaleChangeAmount, scaleChangeDuration))
                    .Append(printerView.DOScale(_startPrinterScale, scaleChangeDuration))
                    .OnComplete(ResetPrinter);
            }
            _isDone = true;
        }

        private async void ResetPrinter()
        {
            printerView.localScale = _startPrinterScale;
            
            var files = Instantiate(prefabFiles, spawnPoint.position, Quaternion.Euler(90,0, 0));
            await files.MoveIn(_endPoint, endPoint, 1);
            
            _officeFileses.Push(files);
            _endPoint.y += files.transform.localScale.y * 0.015f; // todo мэджик 0.015f... фрефаб "гг"
        }

        public async void PickUp(Transform parentTransform)
        {
            var point = parentTransform.position;
            foreach (var files in _officeFileses)
            {
                await files.MoveIn(point, parentTransform, 1f);
                point.y += files.transform.localScale.y * 0.015f; // todo мэджик 0.015f... фрефаб "гг"
            }
            
            _officeFileses.Clear();
            await StartPrinting();
        }
    }
}