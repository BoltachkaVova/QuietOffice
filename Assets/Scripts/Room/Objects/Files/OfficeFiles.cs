using Cysharp.Threading.Tasks;
using DG.Tweening;
using Inventory;
using UnityEngine;

namespace Room
{
    public class OfficeFiles : InventoryBase
    {
        public override async UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0f)
        {
            await transform.DOJump(point, jumpPower, 1, randomDuration == 0 ? duration : randomDuration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(rotation, duration)).SetEase(Ease.Linear)
                .OnComplete(() => transform.parent = parent);
        }
    }
}