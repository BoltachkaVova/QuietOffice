using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using Inventory;
using Pool;
using Signals;
using Signals.Trigger;
using UnityEngine;

namespace Triggers.Action
{
    public class Printer : BaseTriggerAction
    {
        [Header("Special settings")]
        
        [Header("Settings Prefabs")]
        [SerializeField] private InventoryBase[] prefabsFiles;
        [SerializeField] private int countSpawnFiles = 20;
        [SerializeField] private int countGenerateFiles = 40;
        [SerializeField] private Vector3 startRotation;
        
        [Header("Points")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform breakPoint;
        
        [Header("Settings Animation")]
        [SerializeField] private Transform printerView;
        [SerializeField] private float distanceBetweenFiles = 0.03f;
        [SerializeField] private float timeSpawnFiles = 1f;
        [SerializeField] private float shakeForce = 0.05f; 
        [SerializeField] private float shakeRandomness = 90f; 
        [SerializeField] private int shakeVibrato = 10;
        [SerializeField] private Vector3 scaleChangeAmount = new Vector3(0.2f, 0.2f, 0f);
        
        private float shakeDuration;  
        private float scaleChangeDuration;
        private bool _isBreak = false;
        
        private Sequence printerSequence;
        
        private Vector3 _startPrinterScale;
        private Vector3 _endPoint;
        
        private TypeInventory _type = TypeInventory.OfficeFiles;
        private Stack<InventoryBase> _officeFileses =  new Stack<InventoryBase>(20);
        private Pool<InventoryBase> _pool;

        private void Awake()
        {
            _pool = new Pool<InventoryBase>(spawnPoint);
            
            foreach (var file in prefabsFiles)
                _pool.GeneratePool(file, countGenerateFiles);
        }
        
        private void Start()
        {
            _startPrinterScale = printerView.localScale;
            shakeDuration = scaleChangeDuration = timeSpawnFiles * 0.5f;
           
            StartPrinting().Forget();
        }

        private void OnDestroy()
        {
            printerSequence.Kill();
        }

        private async UniTaskVoid StartPrinting()
        {
            TriggerActive(false);
            
            _endPoint = endTransform.position;
            var count = countSpawnFiles;

            if (_isBreak)
            {
                BrokenPrinter();
                return;
            }
            
            while (!IsActiveTrigger)
            {
                if(--count <= 0) 
                    TriggerActive(true); 
                
                printerSequence = DOTween.Sequence();
                await printerSequence.Append(printerView.DOShakePosition(shakeDuration, shakeForce, shakeVibrato, shakeRandomness))
                    .Join(printerView.DOScale(_startPrinterScale + scaleChangeAmount, scaleChangeDuration))
                    .Append(printerView.DOScale(_startPrinterScale, scaleChangeDuration)).OnStart(ResetPrinter);
            }
        }

        private void ResetPrinter()
        {
            if (!_pool.TryGetObject(out InventoryBase files, _type)) return;
            
            printerView.localScale = _startPrinterScale;
            
            files.transform.rotation = Quaternion.Euler(startRotation);
            files.Used(true);
            files.Throw(_endPoint,endTransform.forward).Forget();
                
            _officeFileses.Push(files);
            _endPoint.y += files.transform.localScale.y * distanceBetweenFiles;
        }

        private void BrokenPrinter()
        {
            
        }

        public override async void PickUp(Transform parentTransform) 
        {
            var point = parentTransform.position;
            
            foreach (var files in _officeFileses)
            {
                await files.Throw(point ,parentTransform.forward, parentTransform);
                point.y += files.transform.localScale.y * 0.02f;
            }
            
            _officeFileses.Clear();
            StartPrinting().Forget();
            
            _signal.Fire(new InfoSignal(nameTrigger, textInfo));
            _signal.Fire<ScatterSignal>();
        }


        public void ReturnInWorkState()
        {
            _type = TypeInventory.OfficeFiles;
            _isBreak = false;

            StartPrinting().Forget();
        }

        public override async UniTask Break() 
        {
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
            
            _isBreak = true;
            TriggerActive(false); 
        }

        public override async UniTask Change() 
        {
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
            
            _type = TypeInventory.TrashOfficeFiles;
            TriggerActive(false); 
        }

        protected override void PlayerTriggerEnter()
        {
            _signal.Fire(new SelectTriggerActionSignal(this, breakPoint));
        }

        protected override void PlayerTriggerExit()
        {
           
        }
    }
}