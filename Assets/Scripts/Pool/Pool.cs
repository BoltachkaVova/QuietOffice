using System.Collections.Generic;
using System.Linq;
using Enums;
using Inventory;
using UnityEngine;

namespace Pool
{
    public class Pool<T> where T : InventoryBase
        {
            private readonly List<T> _pool = new List<T>(20);
            private readonly Transform _parent;
            
            public Pool(Transform parent)
            {
                _parent = parent;
            }

            public void GeneratePool(T prefab, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var item = Object.Instantiate(prefab, _parent);
                    item.Used(false);
                    
                    _pool.Add(item);
                }
            }
            
            public bool TryGetObject(out T item, TypeInventory type)
            {
                item = _pool.FirstOrDefault(ob => !ob.IsUse && ob.TypeInventory == type);
                return item != null;
            }

        }
}