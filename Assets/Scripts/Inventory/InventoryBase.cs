using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Inventory
{
    public abstract class InventoryBase : MonoBehaviour
    {
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected float jumpPower = 2f;
        [SerializeField] protected int lifeTimeSeconds;
        [SerializeField] protected TypeInventory typeInventory;

        protected bool _isUse = true;
        public bool IsUse => _isUse;
        public TypeInventory TypeInventory => typeInventory;

        public abstract UniTask Throw(Vector3 point, Vector3 rotation, Transform parent = null, float randomDuration = 0f);


        public void Used(bool isOn)
        {
            _isUse = isOn;
            gameObject.SetActive(isOn);
        }


    }
}