using UnityEngine;

namespace Signals
{
    public struct BusySignal
    {
        private readonly Transform transformRoom;

        public BusySignal(Transform transformRoom)
        {
            this.transformRoom = transformRoom;
        }

        public Transform TransformRoom => transformRoom;
    }
}