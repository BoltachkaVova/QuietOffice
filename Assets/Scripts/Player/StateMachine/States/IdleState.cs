using Interfases;

namespace Player
{
    public class IdleState : IState
    {
        private readonly Joystick _joystick;
        private readonly PlayerAnimator _animator;

        public IdleState(Joystick joystick, PlayerAnimator animator)
        {
            _joystick = joystick;
            _animator = animator;
        }

        public void Enter()
        {
            _joystick.gameObject.SetActive(false);
            _joystick.OnPointerUp(null);
            _animator.Move(0);
        }

        public void Update()
        {
           
        }

        public void Exit()
        {
            _joystick.gameObject.SetActive(true);
        }
        
    }
}