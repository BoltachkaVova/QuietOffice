using System;
using Room;
using UnityEngine;

namespace Triggers
{
    public class WorkTrigger : TriggerBase
    {
        private Chair _chair;
        public Chair Chair => _chair;

        private void Awake()
        {
            _chair = GetComponentInParent<Chair>();
        }
    }
}