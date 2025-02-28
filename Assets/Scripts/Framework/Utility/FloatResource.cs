using System;
using UnityEngine;

namespace com.dhcc.framework
{
    /// <summary>
    /// A class meant to be used for resources like health, shield, mana, etc.
    /// </summary>
    [Serializable]
    public class FloatResource
    {
        [Header("Settings")]
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }

        private float currentValue;
        public float CurrentValue
        {
            get => currentValue;
            set => currentValue = Mathf.Clamp(value, MinValue, MaxValue);
        }

        public float Percentage => CurrentValue / MaxValue;
        public bool IsMin => CurrentValue.IsEqual(MinValue);
        public bool IsMax => CurrentValue.IsEqual(MaxValue);
    }
}