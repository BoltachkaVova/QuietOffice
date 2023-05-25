using System;
using Interfases;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerStateMachine : IInitializable, ITickable, IDisposable
    {
        private bool _isTick;
        
        private IState _currentState;
        private readonly ActiveState _activeState;

        public PlayerStateMachine(ActiveState activeState)
        {
            _activeState = activeState;
        }

        public void Initialize()
        {
            ChangeState(_activeState);
            _isTick = true;
        }

        public void Tick()
        {
            if(!_isTick) return;
            _currentState.Update();
        }

        public void Dispose()
        {
            
        }

        private void ChangeState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}