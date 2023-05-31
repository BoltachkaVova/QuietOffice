using UnityEngine;

namespace Signals
{
    public struct WorkSignal
    {
        private readonly Transform _trigger;
        private readonly Transform _objTransform;

        public Transform TriggerTransform => _trigger;
        public Transform ObjTransform => _objTransform;
        
        public WorkSignal(Transform trigger, Transform objTransform = null)
        {
            _trigger = trigger;
            _objTransform = objTransform;
        }
    }
}