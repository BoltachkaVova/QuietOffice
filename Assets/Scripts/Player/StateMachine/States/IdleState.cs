using Interfases;

namespace Player
{
    public class IdleState : IState
    {

        private readonly PlayerAnimator _animator;

        public IdleState( PlayerAnimator animator)
        {
            _animator = animator;
        }

        public void Enter()
        {
            _animator.Move(0);
        }

        public void Update()
        {
           
        }

        public void Exit()
        {
            
        }
        
    }
}