using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Inventory
{
    public class Paper : InventoryBase
    {
        public override UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}