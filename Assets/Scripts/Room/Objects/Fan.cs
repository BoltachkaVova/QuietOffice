using DG.Tweening;
using UnityEngine;

namespace Room
{
    public class Fan : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private void Start()
        {
            transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
}