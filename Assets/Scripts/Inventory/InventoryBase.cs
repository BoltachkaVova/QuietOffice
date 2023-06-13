using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Inventory
{
    public abstract class InventoryBase : MonoBehaviour
    {
        public abstract UniTask Throw(Transform parent, Vector3 point, Vector3 rotation, float duration);
    }
}