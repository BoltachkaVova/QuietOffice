using Interfases;
using UnityEngine;

namespace Signals
{
    public struct BreakStateSignal
    {
        private readonly Transform _transformObj;
        private readonly IActions _actions;
        public Transform TransformObj => _transformObj;
        public IActions Actions => _actions;

        public BreakStateSignal(Transform transformObj, IActions actions)
        {
            _transformObj = transformObj;
            _actions = actions;
        }
    }
}