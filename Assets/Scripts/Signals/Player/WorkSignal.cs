using UnityEngine;

namespace Signals
{
    public struct WorkSignal
    {
        private readonly GameObject gameObject;

        public GameObject Component => gameObject;
        
        public WorkSignal(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}