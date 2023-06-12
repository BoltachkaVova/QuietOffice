using DG.Tweening;
using UnityEngine;

namespace Room
{
    public class TriggerDoor : MonoBehaviour
    {
        [SerializeField] private float rotationDuration = 1f;
        [SerializeField] private float openAngle = 110;
        
        private Door _door;
        private Quaternion _closeRotation;
        
        private bool _isOpen = false;
        
        private Tweener _doorTween;
        
        private void Awake()
        {
            _door = GetComponentInChildren<Door>();
            _closeRotation = transform.rotation;
        }
        
        private void OnTriggerEnter(Collider other)
        { 
            OpenDoor(other.transform.position);
        }

        private void OnTriggerExit(Collider other)
        {
            _isOpen = true;
            CloseDoor();
        }

        private void OpenDoor(Vector3 character)
        {
            if (_isOpen) return;
            
            var transformDoor = _door.transform;
            Vector3 characterDirection = character - transformDoor.position;
            float dotProduct = Vector3.Dot(characterDirection.normalized, transformDoor.forward);

            float targetAngle = dotProduct > 0f ? -openAngle : openAngle;
            _doorTween = _door.transform.DORotate(_closeRotation.eulerAngles + new Vector3(0f, targetAngle, 0f), 
                rotationDuration).SetEase(Ease.Linear);
        }

        private void CloseDoor()
        {
            if (_isOpen)
            {
                _doorTween = _door.transform.DORotate(_closeRotation.eulerAngles, rotationDuration)
                    .SetEase(Ease.Linear).OnComplete(() => _isOpen = false);
            }
        }
        
    }
}