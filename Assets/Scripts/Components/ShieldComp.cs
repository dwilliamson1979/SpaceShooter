using com.dhcc.framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class ShieldComp : MonoBehaviour, IDamageReceiver
    {
        [Header("Settings")]
        [field: SerializeField] public IntResource Shield { get; private set; }
        [field: SerializeField] public int DamagePriority { get; private set; } = 0;
        [SerializeField] protected List<EDamageType> AcceptableDamage = new();

        public event Action<int, ShieldComp> OnShieldChanged;

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

            if (damageType == EDamageType.Shield)
                amount *= -1;

            int previousValue = Shield.CurrentValue;
            Shield.CurrentValue -= amount;

            int delta = previousValue - Shield.CurrentValue;
            if (Math.Abs(delta) > 0)
                OnShieldChanged?.Invoke(delta, this);

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