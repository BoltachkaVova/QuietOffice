using UnityEngine;

namespace Player
{
    public class PlayerAnimator
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int StartWork = Animator.StringToHash("StartWork");
        private static readonly int StopWork = Animator.StringToHash("StopWork");
        private static readonly int ThrowAt = Animator.StringToHash("ThrowAtMinion");
        private static readonly int Throw = Animator.StringToHash("Throw");
        
        private readonly Animator _animator;
        

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void Move(float speed)
        {
            _animator.SetFloat(Speed, speed);
        }

        public void StartedWork()
        {
            _animator.SetTrigger(StartWork);
        }

        public void StayWork()
        {
            _animator.SetTrigger(StopWork);
        }

        public void ThrowAtMinion()
        {
            _animator.SetTrigger(ThrowAt);
        }

        public void ThrowObj()
        {
            _animator.SetTrigger(Throw);
        }
    }
}