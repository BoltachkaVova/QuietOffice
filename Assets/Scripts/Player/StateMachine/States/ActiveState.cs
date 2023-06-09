﻿using Interfases;
using UnityEngine;

namespace Player
{
    public class ActiveState : IState
    {
        private float _speed = 2.2f;
        private float _speedRotate = 7f;
        
        private readonly PlayerAnimator _animator;
        private readonly Joystick _joystick;
        private readonly Player _player;

        public ActiveState(PlayerAnimator animator, Joystick joystick, Player player)
        {
            _animator = animator;
            _joystick = joystick;
            _player = player;
        }
        
        public void Enter()
        {
            _joystick.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (_joystick.Direction != Vector2.zero)
            {
                Transform transform;
                
                var direction = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
                (transform = _player.transform).rotation = Quaternion.Lerp(_player.transform.rotation,
                    Quaternion.LookRotation(direction),
                    _speedRotate * Time.deltaTime);

                var magnitude = _joystick.Direction.magnitude;
                transform.position += transform.forward * (_speed * Time.deltaTime * magnitude);
                _animator.Move(magnitude);
                
                return;
            }
            
            _animator.Move(0);
        }

        public void Exit()
        {
            _joystick.gameObject.SetActive(false);
            _joystick.OnPointerUp(null);
        }
    }
}