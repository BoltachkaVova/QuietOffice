using Cysharp.Threading.Tasks;
using DG.Tweening;
using Inventory;
using UnityEngine;

namespace Room
{
    public class OfficeFiles : InventoryBase
    {
        public override async UniTask Throw(Transform parent, Vector3 point, Vector3 rotation, float duration)
        {
            await transform.DOJump(point, 2f, 1, duration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(rotation, duration)).SetEase(Ease.Linear)
                .OnComplete(() => transform.parent = parent);
        }
    }
}