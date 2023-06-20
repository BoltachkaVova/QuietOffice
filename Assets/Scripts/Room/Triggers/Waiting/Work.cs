using Signals;

namespace Room
{
    public class Work : TriggerWaitingBase
    {
        private Chair _chair;

        private void Awake()
        {
            _chair = GetComponentInChildren<Chair>();
        }

        protected override void PlayerTriggerEnter()
        {
            _signal.Fire(new WorkStateSignal(transform, _chair.transform));
        }

        protected override void PlayerTriggerExit()
        {
            _player.CloseProgress();
        }
    }
}