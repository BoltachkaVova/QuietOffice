using Enums;
using UnityEngine;

namespace Signals
{
    public struct ScatterHereSignal
    {
        private readonly Transform _transformRoom;
        private readonly TypeInventory _type;
        
        public Transform TransformRoom => _transformRoom;
        public TypeInventory Type => _type;

        public ScatterHereSignal(Transform transformRoom, TypeInventory type)
        {
            _transformRoom = transformRoom;
            _type = type;
        }
        
    }
}