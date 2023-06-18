using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using Interfases;
using Inventory;
using Pool;
using UnityEngine;

namespace Room
{
    public class Printer : TriggerWaitingBase, IActions
    {
        [Header("Settings Prefabs")]
        [SerializeField] private OfficeFiles[] prefabsFiles;
        [SerializeField] private int countSpawnFiles = 20;
        [SerializeField] private int countGenerateFiles = 40;
        [SerializeField] private Vector3 startRotation;
        
        [Header("Points")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform endTransform;
        
        [Header("Settings Animation")]
        [SerializeField] private Transform printerView;
        [SerializeField] private float timeSpawnFiles = 1f;
        [SerializeField] private float shakeForce = 0.05f; 
        [SerializeField] private float shakeRandomness = 90f; 
        [SerializeField] private int shakeVibrato = 10;
        [SerializeField] private Vector3 scaleChangeAmount = new Vector3(0.2f, 0.2f, 0f);
        private float shakeDuration;  
        private float scaleChangeDuration;
        private Sequence printerSequence;
        
        private Vector3 _startPrinterScale;
        private Vector3 _endPoint;

        private bool _isBreak;
        
        private TypeInventory _type;
        private Stack<OfficeFiles> _officeFileses =  new Stack<OfficeFiles>(20);
        private Pool<OfficeFiles> _pool;

        private void Awake()
        {
            _pool = new Pool<OfficeFiles>(spawnPoint);
            
            foreach (var file in prefabsFiles)
                _pool.GeneratePool(file, countGenerateFiles);
            
            ReturnToWorkingCondition();
        }

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
            isActive = false;
            _endPoint = endTransform.position;
            
            var count = countSpawnFiles;
            while (!_isBreak)
            {
                count--;
                if(count == 0) Break(true);
                
                printerSequence = DOTween.Sequence();
                await printerSequence
                    .Append(printerView.DOShakePosition(shakeDuration, shakeForce, shakeVibrato, shakeRandomness))
                    .Join(printerView.DOScale(_startPrinterScale + scaleChangeAmount, scaleChangeDuration))
                    .Append(printerView.DOScale(_startPrinterScale, scaleChangeDuration)).OnStart(ResetPrinter);
            }
            isActive = true;
        }

        private void ResetPrinter()
        {
            printerView.localScale = _startPrinterScale;

            if (!_pool.TryGetObject(out OfficeFiles files, _type)) return;
            
            files.transform.rotation = Quaternion.Euler(startRotation);
            files.Used(true);
            files.Throw(_endPoint,endTransform.forward).Forget();
                
            _officeFileses.Push(files);
            _endPoint.y += files.transform.localScale.y * 0.015f;
        }

        public async void PickUp(Transform parentTransform)
        {
            var point = parentTransform.position;
            foreach (var files in _officeFileses)
            {
                await files.Throw(point ,parentTransform.forward, parentTransform);
                point.y += files.transform.localScale.y * 0.015f; 
            }
            
            _officeFileses.Clear();
            await StartPrinting();
        }

        public void ReturnToWorkingCondition() // починить может кто-то 
        {
            Break(false);
            Change(); 
        }

        public void Break(bool isOn) // todo игрок может сломать принтер 
        {
            _isBreak = isOn;
            // todo сделать анимацию принтера что он тип тупо трясется и все. + звук мб
        }

        public void Change() 
        {
            _type = _type == TypeInventory.Files ? TypeInventory.TrashFiles : TypeInventory.Files;
        }
    }
}