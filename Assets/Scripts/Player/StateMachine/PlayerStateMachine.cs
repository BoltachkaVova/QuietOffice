using System;
using Interfases;
using Signals;
using Zenject;

namespace Player
{
    public class PlayerStateMachine : IInitializable, ITickable, IDisposable
    {
        private bool _isTick;
        
        private IState _currentState;
        private readonly ActiveState _activeState;
        private readonly WorkState _workState;
        private readonly ThrowState _throwState;
        private readonly SignalBus _signalBus;
        private readonly IdleState _idleState;

        public PlayerStateMachine(ActiveState activeState, WorkState workState,
            ThrowState throwState, SignalBus signalBus, IdleState idleState)
        {
            _activeState = activeState;
            _workState = workState;
            _throwState = throwState;
            _signalBus = signalBus;
            _idleState = idleState;
        }

        public void Initialize()
        {
            ChangeState(_activeState); // todo временно!!!
            _isTick = true;
            
            _signalBus.Subscribe<WorkStateSignal>(OnWorked);
            _signalBus.Subscribe<ActiveStateSignal>(OnActive);
            _signalBus.Subscribe<ThrowStateSignal>(OnThrow);
            _signalBus.Subscribe<IdleStateSignal>(OnIdle);
        }
        
        public void Tick()
        {
            if(!_isTick) return;
            _currentState.Update();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<WorkStateSignal>(OnWorked);
            _signalBus.Unsubscribe<ActiveStateSignal>(OnActive);
            _signalBus.Unsubscribe<ThrowStateSignal>(OnThrow);
            _signalBus.Unsubscribe<IdleStateSignal>(OnIdle);
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
        
        private void OnActive()
        {
            ChangeState(_activeState);
        }
        
        private void OnThrow()
        {
            ChangeState(_throwState);   
        }
        
        
        private void OnIdle()
        {
            ChangeState(_idleState);
        }

    }
    
}