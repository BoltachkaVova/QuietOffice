using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory
{
    public class Banana : InventoryBase
    {
        public override async UniTask Throw(Transform parent, Vector3 point, Vector3 rotation, float duration)
        {
           await transform.DOJump(point, 4f, 1, duration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(rotation,duration, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
        }
    }
}