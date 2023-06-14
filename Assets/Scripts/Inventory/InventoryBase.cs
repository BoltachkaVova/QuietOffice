using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Inventory
{
    public abstract class InventoryBase : MonoBehaviour
    {
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected float jumpPower = 2f;
        
        public abstract UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0f);
    }
}