using Cinemachine;
using UnityEngine;
using Zenject;

namespace QuietOffice.Camera
{
    public class CameraTracking : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        private Player.Player _player;
        
        
        [Inject]
        public void Construct(Player.Player player, CinemachineVirtualCamera camera)
        {
            _player = player;
            _camera = camera;
        }
        

        private void Start()
        {
            SetOnPlayer();
        }

        private void SetOnPlayer()
        {
            var playerTransform = _player.transform;
            _camera.m_Follow = playerTransform;
            _camera.m_LookAt = playerTransform;
        }
    }
}