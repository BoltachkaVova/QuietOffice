using System;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        [Inject]
        public void Construct(CinemachineVirtualCamera camera)
        {
            _camera = camera;
        }

        private void Start()
        {
            SetCamera();
        }

        private void SetCamera()
        {
            _camera.m_Follow = transform;
            _camera.m_LookAt = transform;
        }
    }
}