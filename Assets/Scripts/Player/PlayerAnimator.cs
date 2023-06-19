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
        private static readonly int UpTrash = Animator.StringToHash("PickUpTrash");

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

        public void ThrowAtEmployees()
        {
            _animator.SetTrigger(ThrowAt);
        }

        public void ThrowObject()
        {
            _animator.SetTrigger(Throw);
        }

        public void PickUpTrash()
        {
            _animator.SetTrigger(UpTrash);
        }
    }
}