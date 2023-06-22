using System;
using Interfases;
using Signals;
using Zenject;

namespace Player
{
    public class PlayerStateMachine : IInitializable, ITickable, IDisposable
    {
        private bool _isTick = true;
        
        private IState _currentState;
        
        private readonly ActiveState _activeState;
        private readonly WorkState _workState;
        private readonly ThrowState _throwState;
        private readonly SignalBus _signalBus;
        private readonly IdleState _idleState;
        private readonly ActionsState _actionsState;

        public PlayerStateMachine(ActiveState activeState, WorkState workState,
            ThrowState throwState, SignalBus signalBus, IdleState idleState, ActionsState actionsState)
        {
            _activeState = activeState;
            _workState = workState;
            _throwState = throwState;
            _signalBus = signalBus;
            _idleState = idleState;
            _actionsState = actionsState;
        }

        public void Initialize()
        {
            ChangeState(_activeState); // todo временно!!!
            
            _signalBus.Subscribe<WorkStateSignal>(OnWorked);
            _signalBus.Subscribe<ActiveStateSignal>(OnActive);
            _signalBus.Subscribe<ThrowStateSignal>(OnThrow);
            _signalBus.Subscribe<IdleStateSignal>(OnIdle);
            _signalBus.Subscribe<ActionStateSignal>(OnActions);
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
            _signalBus.Unsubscribe<ActionStateSignal>(OnActions);
        }
        
        private void ChangeState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        
        private void OnWorked()
        {
            _isTick = false;
            ChangeState(_workState);
        }
        
        private void OnActive()
        {
            _isTick = true;
            ChangeState(_activeState);
        }
        
        private void OnThrow()
        {
            _isTick = true;
            ChangeState(_throwState);   
        }

        private void OnIdle()
        {
            _isTick = false;
            ChangeState(_idleState);
        }
        
        private void OnActions()
        {
            _isTick = false;
            ChangeState(_actionsState);
        }

    }
    
}