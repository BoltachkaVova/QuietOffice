using UnityEngine;

namespace Player
{
    public class PlayerAnimator
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int StartWork = Animator.StringToHash("StartWork");
        private static readonly int StopWork = Animator.StringToHash("StopWork");
        
        
        private readonly Animator _animator;
        

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void Move(float speed)
        {
            _animator.SetFloat(Speed, speed);
        }

        public void Work()
        {
            _animator.SetTrigger(StartWork);
        }
    }
}