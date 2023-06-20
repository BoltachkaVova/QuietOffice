using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory
{
    public class OfficeFiles : InventoryBase
    {
        public override async UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0)
        {
            await transform.DOJump(point, jumpPower, 1, randomDuration == 0 ? duration : randomDuration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(rotation, duration)).SetEase(Ease.Linear).OnComplete(()=> transform.parent = parent);
        }
        
    }
}