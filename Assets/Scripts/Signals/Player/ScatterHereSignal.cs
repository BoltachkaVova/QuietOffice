using UnityEngine;

namespace Signals
{
    public struct ScatterHereSignal
    {
        private readonly Transform transformRoom;

        public ScatterHereSignal(Transform transformRoom)
        {
            this.transformRoom = transformRoom;
        }

        public Transform TransformRoom => transformRoom;
    }
}