using Cysharp.Threading.Tasks;
using DG.Tweening;
using Signals;
using Triggers;
using UnityEngine;
using Zenject;


namespace Player
{
    public class Player : MonoBehaviour
    {
        private PlayerAnimator _animator;
        private Joystick _joystick;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(PlayerAnimator animator, Joystick joystick, SignalBus signalBus)
        {
            _animator = animator;
            _joystick = joystick;
            _signalBus = signalBus;
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Work component))
            {
                //todo добавить задержку и прогресс бар
                _signalBus.Fire(new WorkSignal(component.gameObject));
            }
        }
    }
}