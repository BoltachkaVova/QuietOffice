using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Room
{
    public class ZoneOpenClosedDoor : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private float euler;
        
        private Door _door;
        private Vector3 _closed;
        private Vector3 _opened;

        private bool _isOpened;
        private void Awake()
        {
            _door = GetComponentInChildren<Door>();
        }

        private void Start()
        {
            _closed = new Vector3(0,-euler,0);
            _opened = new Vector3(0,euler,0);
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<Player.Player>() || _isOpened) return;

            await Door(_opened);
            _isOpened = true;
        }

        private async void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<Player.Player>() || !_isOpened) return;
            await Door(_closed);
            _isOpened = false;
        }

        private async UniTask Door(Vector3 vector3)
        {
           await _door.transform.DORotate(vector3, duration, RotateMode.WorldAxisAdd);
        }
        
    }
}