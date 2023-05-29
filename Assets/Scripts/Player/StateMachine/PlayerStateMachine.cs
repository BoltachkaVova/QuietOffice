using System;
using Interfases;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerStateMachine : IInitializable, ITickable, IDisposable
    {
        private bool _isTick;
        
        private IState _currentState;
        private readonly ActiveState _activeState;
        private readonly WorkState _workState;
        private readonly SignalBus _signalBus;

        public PlayerStateMachine(ActiveState activeState, WorkState workState, SignalBus signalBus)
        {
            _activeState = activeState;
            _workState = workState;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            ChangeState(_activeState);
            _isTick = true;
            
            _signalBus.Subscribe<WorkSignal>(OnWorked);
        }
        
        public void Tick()
        {
            if(!_isTick) return;
            _currentState.Update();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<WorkSignal>(OnWorked);
        }

        private void ChangeState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        
        private void OnWorked()
        {
            ChangeState(_workState);
        }
    }
}