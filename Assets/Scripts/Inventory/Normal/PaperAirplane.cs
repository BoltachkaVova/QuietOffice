using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Inventory.Normal
{
    public class PaperAirplane : InventoryBase 
    {
        public override async UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0)
        {
            await transform.DOJump(point, jumpPower, 1, duration).SetEase(Ease.InOutSine).SetEase(Ease.Linear)
                .OnStart(()=> transform.parent = parent);
            
            await UniTask.Delay(lifeTimeSeconds * 1000);
            Used(false);
        }

    }
}