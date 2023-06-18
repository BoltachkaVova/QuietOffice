using Interfases;

namespace Player
{
    public class IdleState : IState
    {

        private readonly PlayerAnimator _animator;
        private readonly Player _player;

        public IdleState(PlayerAnimator animator, Player player)
        {
            _animator = animator;
            _player = player;
        }

        public void Enter()
        {
            _animator.Move(0);
            _player.OnIgnore(true);
        }

        public void Update()
        {
           
        }

        public void Exit()
        {
            
        }
        
    }
}