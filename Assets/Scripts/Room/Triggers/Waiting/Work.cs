namespace Room
{
    public class Work : TriggerWaitingBase
    {
        private Chair _chair;
        public Chair Chair => _chair;

        private void Awake()
        {
            _chair = GetComponentInChildren<Chair>();
        }
        
    }
}