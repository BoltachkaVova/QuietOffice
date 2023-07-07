using DG.Tweening;
using UnityEngine;

namespace Triggers.Objects.Fan
{
    public class Fan : MonoBehaviour
    {
        [Header("Settings fan")]
        [SerializeField] private float normalSpeedFan = 1f;
        [SerializeField] private float crazySpeedFan = 2f;
        [SerializeField] private float durationTransitionBetweenSpeeds = 1f;

        [Header("Settings particle")]
        [SerializeField] private float minHeight = 0.5f;
        [SerializeField] private float durationMoveY = 3f;
        
        private float _maxHeight;
        
        private Sequence _fanSequence;
        private Sequence _flySequence;
        
        private ParticleSystem _particle;

        private void Awake()
        {
            _maxHeight = transform.position.y;
            _particle = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            StartNormalFans(normalSpeedFan);
        }

        
        private void StartNormalFans(float speed)
        {
            _fanSequence = DOTween.Sequence();
            _fanSequence.Append(transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360)
                .SetLoops(int.MaxValue).SetEase(Ease.Linear));
        }
        

        public void StartFlyObjects()
        {
            _fanSequence.DOTimeScale(crazySpeedFan, durationTransitionBetweenSpeeds);

            _flySequence = DOTween.Sequence();
            _particle.Play();
            
            _flySequence.Append(_particle.transform.DOMoveY(minHeight, durationMoveY))
                .Append(_particle.transform.DOMoveY(_maxHeight, durationMoveY)).SetEase(Ease.Linear).SetLoops(int.MaxValue);
        }

        public void ReturnNormalState()
        {
            _particle.Stop();
            _flySequence.Kill();
            
            _fanSequence.DOTimeScale(normalSpeedFan, durationTransitionBetweenSpeeds);
        }


    }
}