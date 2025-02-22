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
        new public IState<E> CurrentState { get; protected set; }

        protected abstract IState<E> GetState(E state);
        protected abstract void AddState(IState<E> state);

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

        //Will removing this cause issues? Will the base class of FSM.SetState() work when trying to assign the argument of IState to the "new" CurrentState (IState<E>)?
        //public void SetState(IState<E> newState)
        //{
        //    if (newState == null)
        //    {
        //        Debug.LogWarning("StateMachine.SetState received a null argument and will remain in the current state.");
        //        return;
        //    }

        //    CurrentState?.Exit();

        //    CurrentState = newState;
        //    CurrentState.Enter();
        //}
    }
}