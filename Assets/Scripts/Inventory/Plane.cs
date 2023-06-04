using System;
using UnityEngine;

namespace Inventory
{
    public class Plane : InventoryBase
    {
        [SerializeField] private float speed;

        private bool _isActive;

        private void OnEnable()
        {
            _isActive = true;
        }

        private void OnCollisionStay(Collision other)
        {
            _isActive = false;
        }

        private void Update()
        {
            if(!_isActive) return;
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
        
    }
}