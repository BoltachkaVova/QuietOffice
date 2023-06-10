using System;
using Cinemachine;
using Zenject;

namespace QuietOffice.Camera
{
    public class TrackingCamera : IInitializable, IDisposable
    {
        private readonly CinemachineVirtualCamera _camera;
        private readonly Player.Player _player;
        
        public TrackingCamera (Player.Player player, CinemachineVirtualCamera camera)
        {
            _player = player;
            _camera = camera;
        }
        
        public void Initialize()
        {
            SetOnPlayer();
        }

        public void Dispose()
        {
            
        }

        private void SetOnPlayer()
        {
            var playerTransform = _player.transform;
            _camera.m_Follow = playerTransform;
            _camera.m_LookAt = playerTransform;
        }

       
    }
}