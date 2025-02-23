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

    public class FSM<E> : FSM where E : Enum
    {
        [SerializeField] private Dictionary<E, IState<E>> states = new();

        public void AddState(IState<E> state)
        {
            if (states.ContainsKey(state.StateID))
            {
                Debug.Log($"(AddState) {this} already contains the {state.StateID} state.");
                return;
            }

            states[state.StateID] = state;
        }

        private IState<E> GetState(E stateID)
        {
            return states.ContainsKey(stateID) ? states[stateID] : null;
        }

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