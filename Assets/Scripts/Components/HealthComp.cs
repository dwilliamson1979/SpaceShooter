using com.dhcc.core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.components
{
    public class HealthComp : MonoBehaviour, IDamageReceiver
    {
        [Header("Settings")]
        [field: SerializeField] public IntResource Health { get; private set; }
        [field: SerializeField] public int DamagePriority { get; private set; } = 0;
        [SerializeField] protected List<EDamageType> AcceptableDamage = new();

        public event Action<int, HealthComp> OnHealthChanged = delegate { };

        protected virtual void Start()
        {
            ////Maybe create an event system local to the gameobject and call RegisterDamageReceiver?
            //var drc = GetComponent<DamageComp>();
            //if (drc is not null)
            //    drc.Register(this);
        }

        public virtual int TakeDamage(EDamageType damageType, int amount)
        {
            if (!AcceptsDamage(damageType)) return 0;

            if (damageType == EDamageType.Health)
                amount *= -1;

            int previousValue = Health.CurrentValue;
            Health.SetValue(Health.CurrentValue - amount);

            int delta = previousValue - Health.CurrentValue;
            if (Math.Abs(delta) > 0)
                OnHealthChanged(delta, this);

            //A float example:
            //if (Math.Abs(previousValue - CurrentValue) > changeTolerance)
            //OnHealthChanged(this);

            return delta;
        }

        protected bool AcceptsDamage(EDamageType damageType)
        {
            return AcceptableDamage.Contains(damageType);
        }
    }
}