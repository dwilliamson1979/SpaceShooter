using System;
using System.Collections.Generic;
using UnityEngine;
using com.dhcc.framework;

namespace com.dhcc.spaceshooter
{
    public class HealthComp : MonoBehaviour, IDamageReceiver
    {
        [Header("Settings")]
        [field: SerializeField] public IntResource Health { get; private set; }
        [field: SerializeField] public int DamagePriority { get; private set; } = 0;
        [SerializeField] protected List<EDamageType> AcceptableDamage = new();

        public event Action<int, HealthComp> OnHealthChanged;

        private void Awake()
        {
            //Maybe create an event system local to the gameobject and call RegisterDamageReceiver?
            var damageComp = GetComponent<DamageComp>();
            if (damageComp is not null)
                damageComp.Register(this);
        }

        public virtual int TakeDamage(EDamageType damageType, int amount)
        {
            if (!AcceptsDamage(damageType)) return 0;

            if (damageType == EDamageType.Health)
                amount *= -1;

            int previousValue = Health.CurrentValue;
            Health.CurrentValue -= amount;

            int delta = Health.CurrentValue - previousValue;
            if (Math.Abs(delta) > 0)
                OnHealthChanged?.Invoke(delta, this);

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