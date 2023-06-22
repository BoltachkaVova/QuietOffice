using Triggers;
using UnityEngine;

namespace Signals
{
    public struct SelectTriggerActionSignal
    {
        private readonly TriggerAction _triggerAction;
        private readonly Transform _target;
      


        public SelectTriggerActionSignal(TriggerAction triggerAction, Transform target)
        {
            _triggerAction = triggerAction;
            _target = target;
        }

        public TriggerAction Action => _triggerAction;

        public Transform Target => _target;
    }
}