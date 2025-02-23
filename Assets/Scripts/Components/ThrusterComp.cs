using com.dhcc.utility;
using com.dhcc.fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.components
{
    public class ThrusterComp : MonoBehaviour
    {
        #region State Machine Definitions
        private enum EThrusterState { Idle, Thrusting, Recovering, Refueling, Burnout } //Recovering may go away...

        private class ThrusterFSM : FSM<EThrusterState>
        {
            [SerializeField] private Dictionary<EThrusterState, IState<EThrusterState>> states = new();

            public override void AddState(IState<EThrusterState> state)
            {
                if (states.ContainsKey(state.StateID))
                {
                    Debug.Log($"(AddState) {this} already contains the {state.StateID} state.");
                    return;
                }

                states[state.StateID] = state;
            }

            protected override IState<EThrusterState> GetState(EThrusterState stateID)
            {
                return states.ContainsKey(stateID) ? states[stateID] : null;
            }
        }

        private abstract class ThrusterState : IState<EThrusterState>
        {
            public EThrusterState StateID { get; protected set; }
            protected ThrusterComp thruster;

            public ThrusterState(ThrusterComp thrusterComp)
            {
                this.thruster = thrusterComp;
            }

            public virtual void Enter() { }
            public virtual void Exit() { }
            public virtual void FixedUpdate() { }
            public virtual void Update() { }
        }

        private class IdleState : ThrusterState
        {
            public IdleState(ThrusterComp thrusterComp) : base(thrusterComp) { StateID = EThrusterState.Idle; }

            public override void Update()
            {
                if (thruster.wantsToThrust && !thruster.Fuel.IsAtMin)
                {
                    thruster.fsm.SetState(EThrusterState.Thrusting);
                    return;
                }

                if (!thruster.Fuel.IsAtMax)
                {
                    thruster.Fuel.Add(thruster.recoveryRate * Time.deltaTime);
                    thruster.OnFuelChanged?.Invoke();
                    return;
                }
            }
        }

        private class ThrustingState : ThrusterState
        {
            public ThrustingState(ThrusterComp thrusterComp) : base(thrusterComp) { StateID = EThrusterState.Thrusting; }

            public override void Enter()
            {
                thruster.IsThrusting = true;
                thruster.OnThrustChanged?.Invoke();
            }

            public override void Update()
            {
                if (!thruster.wantsToThrust)
                    thruster.fsm.SetState(EThrusterState.Idle);

                thruster.Fuel.Add(-thruster.burnRate * Time.deltaTime);
                thruster.OnFuelChanged?.Invoke();

                if (thruster.Fuel.CurrentValue <= 0f)
                    thruster.fsm.SetState(EThrusterState.Burnout);
            }

            public override void Exit()
            {
                thruster.IsThrusting = false;
                thruster.OnThrustChanged?.Invoke();
            }
        }

        private class BurnoutState : ThrusterState
        {
            private float elapsedRecoveryTime;
            public BurnoutState(ThrusterComp thrusterComp) : base(thrusterComp) { StateID = EThrusterState.Burnout; }

            public override void Enter()
            {
                elapsedRecoveryTime = 0f;
            }

            public override void Update()
            {
                if(thruster.wantsToThrust)
                {
                    elapsedRecoveryTime = 0f;
                    return;
                }

                elapsedRecoveryTime += Time.deltaTime;
                if (elapsedRecoveryTime >= thruster.burnoutRecoveryTime)
                    thruster.fsm.SetState(EThrusterState.Idle);
            }
        }
        #endregion

        [Header("Settings")]
        [field: SerializeField] public FloatResource Fuel { get; private set; }
        [Tooltip("Burn rate of the fuel per second")]
        [SerializeField] private float burnRate = 0.5f;
        [SerializeField] private float recoveryRate = 0.5f;
        [SerializeField] private float burnoutRecoveryTime = 2f;        
        [field: SerializeField] public float SpeedModifier { get; private set; } = 0.25f;
        [SerializeField] private ThrusterFSM fsm;

        public bool IsThrusting { get; private set; }
        public event Action OnThrustChanged;
        public event Action OnFuelChanged;

        private bool wantsToThrust;

        private void Start()
        {
            fsm = new ThrusterFSM();
            fsm.AddState(new IdleState(this));
            fsm.AddState(new ThrustingState(this));
            fsm.AddState(new BurnoutState(this));

            fsm.SetState(EThrusterState.Idle);
        }

        private void Update()
        {
            fsm.Update();

            ////TODO This class is a good candidate for a state machine {Idle, Thrusting, Recovering, Refueling, Burnout}?

            //if(!isInRecoveryMode)
            //{
            //    if (wantsToThrust && Fuel.CurrentValue > 0f)
            //    {
            //        if (!isThrusting)
            //            Activate();

            //        Fuel.Add(-burnRate * Time.deltaTime);
            //        OnThrusterChanged?.Invoke(this);

            //        if (Fuel.CurrentValue <= 0f)
            //            StartCoroutine(BurnoutRecoveryRoutine());

            //        return;
            //    }
                
            //    //Refuel if we are out of fuel or not thrusting.
            //    Refuel();              
            //}
        }

        public void Thrust(bool thrust)
        {
            wantsToThrust = thrust;
        }

        //private void Activate()
        //{
        //    isThrusting = true;
        //    movementComp.SetSpeed(movementComp.DefaultSpeed * (1f + speedModifier));
        //}

        //private void Deactivate()
        //{
        //    isThrusting = false;
        //    movementComp.SetSpeed(movementComp.DefaultSpeed);
        //}

        //private void Refuel()
        //{
        //    if (isThrusting)
        //        Deactivate();

        //    if (Fuel.IsAtMax) return;

        //    Fuel.Add(recoveryRate * Time.deltaTime);
        //    OnThrusterChanged?.Invoke(this);
        //}

        //private IEnumerator BurnoutRecoveryRoutine()
        //{
        //    isInRecoveryMode = true;
        //    Deactivate();

        //    yield return new WaitForSeconds(burnoutRecoveryTime);
        //    isInRecoveryMode = false;
        //}
    }
}