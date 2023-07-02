using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory.Trashes
{
    public class Cup : InventoryBase
    {
        public override async UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0)
        {
            await transform.DOJump(point, jumpPower, 1, duration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(rotation,duration, RotateMode.FastBeyond360)).SetEase(Ease.Linear)
                .OnStart(()=> transform.parent = parent);
            
            await UniTask.Delay(lifeTimeSeconds * 1000);
            Used(false);
        }
        
    }
}