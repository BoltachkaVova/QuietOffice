using System;
using UnityEngine;

namespace Inventory
{
    public class PaperAirplane : InventoryBase
    {
        [SerializeField] private float speed;

        private bool _isActive;

        private void OnEnable()
        {
            _isActive = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            _isActive = false;
            Debug.Log("Collision");
        }

        private void Update()
        {
            if(!_isActive) return;
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
        
    }
}