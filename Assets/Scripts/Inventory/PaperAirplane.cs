using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory
{
    public class PaperAirplane : InventoryBase 
    {
        [SerializeField] private int lifeTimeSeconds = 5;
        
        public override async UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0f)
        {
            await transform.DOJump(point, jumpPower, 1, duration).SetEase(Ease.InOutSine).SetEase(Ease.Linear).OnComplete(ObjectOff().Forget);
        }
        
        private async UniTaskVoid ObjectOff()
        {
            await UniTask.Delay(lifeTimeSeconds * 1000);
            gameObject.SetActive(false);
        }
    }
}