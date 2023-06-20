using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using Interfases;
using Inventory;
using Pool;
using Signals;
using UnityEngine;
using Zenject;

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
        [SerializeField] private float distanceBetweenFiles = 0.03f;
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
        }
        
        private void Start()
        {
            _startPrinterScale = printerView.localScale;
            shakeDuration = scaleChangeDuration = timeSpawnFiles * 0.5f;
            
            ReturnInWorkState();
            StartPrinting().Forget();
        }

        private void OnDestroy()
        {
            printerSequence.Kill();
        }

        private async UniTaskVoid StartPrinting()
        {
            isActiveTrigger = false;
            
            _endPoint = endTransform.position;
            var count = countSpawnFiles;
           
            while (!_isBreak)
            {
                if(--count == 0) _isBreak = true;
                
                printerSequence = DOTween.Sequence();
                await printerSequence
                    .Append(printerView.DOShakePosition(shakeDuration, shakeForce, shakeVibrato, shakeRandomness))
                    .Join(printerView.DOScale(_startPrinterScale + scaleChangeAmount, scaleChangeDuration))
                    .Append(printerView.DOScale(_startPrinterScale, scaleChangeDuration)).OnStart(ResetPrinter);
            }
            
            isActiveTrigger = true;
        }

        private void ResetPrinter()
        {
            printerView.localScale = _startPrinterScale;

            if (!_pool.TryGetObject(out OfficeFiles files, _type)) return;
            
            files.transform.rotation = Quaternion.Euler(startRotation);
            files.Used(true);
            files.Throw(_endPoint,endTransform.forward).Forget();
                
            _officeFileses.Push(files);
            _endPoint.y += files.transform.localScale.y * distanceBetweenFiles;
        }

        private async void PickUp(Transform parentTransform) 
        {
            var point = parentTransform.position;
            foreach (var files in _officeFileses)
            {
                await files.Throw(point ,parentTransform.forward, parentTransform);
                point.y += files.transform.localScale.y * 0.02f;
            }
            
            _officeFileses.Clear();
            ReturnInWorkState();
            
            StartPrinting().Forget();
        }

        private void ReturnInWorkState()
        {
            Break(false);
            Change(true);
        }

        public void Break(bool isOn) // todo игрок может сломать принтер 
        {
            _isBreak = isOn;
        }

        public void Change(bool isOn) // todo игрок может подложить другую бумагу
        {
            _type = isOn ? TypeInventory.Files : TypeInventory.TrashFiles;
        }

        protected override async void PlayerTriggerEnter()
        {
            _signal.Fire<IdleStateSignal>();
            
            PickUp(_player.TransformPoint);
            await UniTask.WaitUntil(() => !isActiveTrigger);
            
            _signal.Fire<ActiveStateSignal>();
            _signal.Fire(new InfoInventorySignal(nameTrigger, textInfo));
        }

        protected override void PlayerTriggerExit()
        {
            _player.CloseProgress();
        }
    }
}