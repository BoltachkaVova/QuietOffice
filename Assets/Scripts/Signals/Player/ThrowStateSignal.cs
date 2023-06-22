using Enums;

namespace Signals
{
    public struct ThrowStateSignal
    {
        private TypeInventory _type;
        public TypeInventory Type => _type;
        public ThrowStateSignal(TypeInventory typeInventory)
        {
            _type = typeInventory;
        }
        
    }
}