using Triggers;
using Triggers.Action;
using UnityEngine;

namespace Signals
{
    public struct SelectTriggerActionSignal
    {
        private readonly BaseTriggerAction baseTriggerAction;
        private readonly Transform _target;
      


        public SelectTriggerActionSignal(BaseTriggerAction baseTriggerAction, Transform target)
        {
            this.baseTriggerAction = baseTriggerAction;
            _target = target;
        }

        public BaseTriggerAction Action => baseTriggerAction;

        public Transform Target => _target;
    }
}