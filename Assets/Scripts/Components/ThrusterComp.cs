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

        public event Action<ThrusterComp> OnThrusterChanged = delegate { };

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
            if(!isInRecoveryMode)
            {
                if (Fuel.CurrentValue > 0f)
                {
                    if (wantsToThrust)
                    {
                        if (!isThrusting)
                            InternalActivate();

                        Fuel.Add(-burnRate * Time.deltaTime);
                        OnThrusterChanged(this);

                        if (Fuel.CurrentValue <= 0f)
                            StartCoroutine(BurnoutRecoveryRoutine());

                        return;
                    }
                }
                
                //Refuel if we are out of fuel or not thrusting.
                Refuel();              
            }
        }

        public void Activate()
        {
            wantsToThrust = true;
        }

        private void InternalActivate()
        {
            isThrusting = true;
            movementComp.SetSpeed(movementComp.DefaultSpeed * (1f + speedModifier));
        }

        public void Deactivate()
        {
            wantsToThrust = false;
        }

        private void InternalDeactivate()
        {
            isThrusting = false;
            movementComp.SetSpeed(movementComp.DefaultSpeed);
        }

        private void Refuel()
        {
            if (isThrusting)
                InternalDeactivate();

            Fuel.Add(recoveryRate * Time.deltaTime);
            OnThrusterChanged(this);
        }

        private IEnumerator BurnoutRecoveryRoutine()
        {
            isInRecoveryMode = true;
            InternalDeactivate();

            yield return new WaitForSeconds(burnoutRecoveryTime);
            isInRecoveryMode = false;
        }
    }
}