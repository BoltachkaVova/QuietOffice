using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Room
{
    public class OfficeFiles : MonoBehaviour
    {
        public async UniTask MoveIn(Vector3 point, Transform parent, float duration)
        {
            await transform.DOJump(point, 2f, 1, duration).SetEase(Ease.InOutSine)
                .Join(transform.DORotate(parent.forward, duration)).SetEase(Ease.Linear)
                .OnComplete(() => transform.parent = parent);
        }

        public void Scatter(Vector3 randomPoint)
        {
            
        }
    }
}