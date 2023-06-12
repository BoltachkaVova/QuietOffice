

namespace Room
{
    public class TriggerWork : TriggerBase
    {
        private Chair _chair;
        public Chair Chair => _chair;

        private void Awake()
        {
            _chair = GetComponentInParent<Chair>();
        }
    }
}