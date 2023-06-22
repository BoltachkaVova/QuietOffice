using UnityEngine;

namespace Signals
{
    public struct ScatterHereSignal
    {
        private readonly Transform _transformRoom;
        
        public Transform TransformRoom => _transformRoom;
        
        public ScatterHereSignal(Transform transformRoom)
        {
            _transformRoom = transformRoom;
        }
        
    }
}