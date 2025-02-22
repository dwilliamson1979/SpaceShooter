using System;

namespace com.dhcc.fsm
{
    public interface IState
    {
        void Enter();
        void Update();
        void FixedUpdate();
        void Exit();
    }

    public interface IState<T> : IState where T : Enum
    {
        T StateID { get; }
    }
}