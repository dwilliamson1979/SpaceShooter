using System;
using UnityEngine;
using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public class ThrusterComp : MonoBehaviour
    {
        #region State Machine Definitions

        private abstract class ThrusterState : IState
        {
            protected ThrusterComp obj;

            public ThrusterState(ThrusterComp thrusterComp) => obj = thrusterComp;

            public virtual void Enter() { }
            public virtual void Exit() { }
            public virtual void FixedUpdate() { }
            public virtual void Update() { }
        }

        private class IdleState : ThrusterState
        {
            public IdleState(ThrusterComp thrusterComp) : base(thrusterComp) { }

            public override void Update()
            {
                if (obj.wantsToThrust && !obj.Fuel.IsAtMin)
                {
                    obj.fsm.SetState(obj.thrustingState);
                    return;
                }

                if (!obj.Fuel.IsAtMax)
                {
                    obj.Fuel.Add(obj.recoveryRate * Time.deltaTime);
                    obj.OnFuelChanged?.Invoke();
                    return;
                }
            }
        }

        private class ThrustingState : ThrusterState
        {
            public ThrustingState(ThrusterComp thrusterComp) : base(thrusterComp) { }

            public override void Enter()
            {
                obj.IsThrusting = true;
                obj.OnThrustChanged?.Invoke();
            }

            public override void Update()
            {
                if (!obj.wantsToThrust)
                    obj.fsm.SetState(obj.idleState);

                obj.Fuel.Add(-obj.burnRate * Time.deltaTime);
                obj.OnFuelChanged?.Invoke();

                if (obj.Fuel.CurrentValue <= 0f)
                    obj.fsm.SetState(obj.burnoutState);
            }

            public override void Exit()
            {
                obj.IsThrusting = false;
                obj.OnThrustChanged?.Invoke();
            }
        }

        private class BurnoutState : ThrusterState
        {
            private float elapsedRecoveryTime;
            public BurnoutState(ThrusterComp thrusterComp) : base(thrusterComp) { }

            public override void Enter()
            {
                elapsedRecoveryTime = 0f;
            }

            public override void Update()
            {
                if(obj.wantsToThrust)
                {
                    elapsedRecoveryTime = 0f;
                    return;
                }

                elapsedRecoveryTime += Time.deltaTime;
                if (elapsedRecoveryTime >= obj.burnoutRecoveryTime)
                    obj.fsm.SetState(obj.idleState);
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
        
        private FSM fsm;
        private IState idleState;
        private IState thrustingState;
        private IState burnoutState;

        public bool IsThrusting { get; private set; }
        public event Action OnThrustChanged;
        public event Action OnFuelChanged;

        private bool wantsToThrust;

        private void Start()
        {
            fsm = new FSM();
            idleState = new IdleState(this);
            thrustingState = new ThrustingState(this);
            burnoutState = new BurnoutState(this);
            fsm.SetState(idleState);
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