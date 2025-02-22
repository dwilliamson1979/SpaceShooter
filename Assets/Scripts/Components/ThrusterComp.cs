using com.dhcc.core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.components
{
    public class ThrusterComp : MonoBehaviour
    {
        [Header("Settings")]
        [field: SerializeField] public FloatResource Fuel { get; private set; }
        [Tooltip("Burn rate of the fuel per second")]
        [SerializeField] public float burnRate = 0.5f;
        [SerializeField] public float recoveryRate = 0.5f;
        [SerializeField] public float burnoutRecoveryTime = 2f;        
        [SerializeField] public float speedModifier = 0.25f;

        public event Action<ThrusterComp> OnThrusterChanged;

        private bool wantsToThrust;
        private bool isThrusting;
        private bool isInRecoveryMode;
        private MovementComp movementComp;

        private void Awake()
        {
            movementComp = GetComponent<MovementComp>();
        }

        private void Update()
        {
            //TODO This class is a good candidate for a state machine {Idle, Thrusting, Recovering, Refueling, Burnout}?

            if(!isInRecoveryMode)
            {
                if (wantsToThrust && Fuel.CurrentValue > 0f)
                {
                    if (!isThrusting)
                        Activate();

                    Fuel.Add(-burnRate * Time.deltaTime);
                    OnThrusterChanged?.Invoke(this);

                    if (Fuel.CurrentValue <= 0f)
                        StartCoroutine(BurnoutRecoveryRoutine());

                    return;
                }
                
                //Refuel if we are out of fuel or not thrusting.
                Refuel();              
            }
        }

        public void Thrust(bool thrust)
        {
            wantsToThrust = thrust;
        }

        private void Activate()
        {
            isThrusting = true;
            movementComp.SetSpeed(movementComp.DefaultSpeed * (1f + speedModifier));
        }

        private void Deactivate()
        {
            isThrusting = false;
            movementComp.SetSpeed(movementComp.DefaultSpeed);
        }

        private void Refuel()
        {
            if (isThrusting)
                Deactivate();

            if (Fuel.IsAtMax) return;

            Fuel.Add(recoveryRate * Time.deltaTime);
            OnThrusterChanged?.Invoke(this);
        }

        private IEnumerator BurnoutRecoveryRoutine()
        {
            isInRecoveryMode = true;
            Deactivate();

            yield return new WaitForSeconds(burnoutRecoveryTime);
            isInRecoveryMode = false;
        }
    }
}