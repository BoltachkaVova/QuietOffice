namespace Room
{
    public class TriggerWaitingWork : TriggerWaitingBase
    {
        private Chair _chair;
        public Chair Chair => _chair;

        private void Awake()
        {
            _chair = GetComponentInChildren<Chair>();
        }

        public void ShowProgress()
        {
            throw new System.NotImplementedException();
        }
    }
}