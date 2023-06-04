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
        private readonly AttackState _attackState;
        private readonly SignalBus _signalBus;

        public PlayerStateMachine(ActiveState activeState, WorkState workState,
            AttackState attackState, SignalBus signalBus)
        {
            _activeState = activeState;
            _workState = workState;
            _attackState = attackState;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            ChangeState(_activeState); // todo временно!!!
            _isTick = true;
            
            _signalBus.Subscribe<WorkSignal>(OnWorked);
            _signalBus.Subscribe<StopWorkSignal>(OnStopWorked);
            
            _signalBus.Subscribe<ThrowSignal>(OnThrow);
            _signalBus.Subscribe<TargetLostSignal>(OnTargetLost);
        }
        

        public void Tick()
        {
            if(!_isTick) return;
            _currentState.Update();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<WorkSignal>(OnWorked);
            _signalBus.Unsubscribe<StopWorkSignal>(OnStopWorked);
            
            _signalBus.Unsubscribe<ThrowSignal>(OnThrow);
            _signalBus.Unsubscribe<TargetLostSignal>(OnTargetLost);
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
        
        private void OnStopWorked()
        {
            ChangeState(_activeState);
        }
        
        private void OnThrow()
        {
            ChangeState(_attackState);   
        }
        
        private void OnTargetLost()
        {
            ChangeState(_activeState);
        }
    }
}