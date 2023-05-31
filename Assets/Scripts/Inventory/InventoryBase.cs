using UnityEngine;

namespace Inventory
{
    public abstract class InventoryBase : MonoBehaviour
    {
        [SerializeField] protected string nameInventory = "Inventory";
        [SerializeField] protected string textInfo= "Info";

        public string NameInventory => nameInventory;
        public string TextInfo => textInfo;
    }
}