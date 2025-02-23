using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace com.dhcc.fsm
{
    public class FSM
    {
        public IState CurrentState { get; protected set; }

        public void SetState(IState newState)
        {
            if (newState == null)
            {
                Debug.LogWarning("StateMachine.SetState received a null argument and will remain in the current state.");
                return;
            }

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update() => CurrentState?.Update();
        public void FixedUpdate() => CurrentState?.FixedUpdate();
    }

    public abstract class FSM<E> : FSM where E : Enum
    {
        protected abstract IState<E> GetState(E state);
        public abstract void AddState(IState<E> state);

        public void SetState(E newState)
        {
            var requestedState = GetState(newState);
            if (requestedState == null)
            {
                Debug.LogWarning($"StateMachine: {newState} not found in the list of available states.");
                return;
            }

            CurrentState?.Exit();
            CurrentState = requestedState;
            CurrentState.Enter();
        }
    }
}