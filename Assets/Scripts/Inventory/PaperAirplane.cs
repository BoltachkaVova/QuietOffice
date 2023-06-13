using System;
using Cysharp.Threading.Tasks;
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

        private void Update() // todo поменять 
        {
            if(!_isActive) return;
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }


        public override async UniTask Throw(Transform parent, Vector3 point, Vector3 rotation, float duration)
        {
            
        }
    }
}