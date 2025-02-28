using System;
using UnityEngine;

namespace com.dhcc.framework
{
    /// <summary>
    /// A class meant to be used for resources like health, shield, mana, etc.
    /// </summary>
    [Serializable]
    public class IntResource
    {
        [Header("Settings")]
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }

        private int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set => currentValue = Mathf.Clamp(value, MinValue, MaxValue);
        }

        public float Percentage => CurrentValue / (float)MaxValue;

        public bool IsMin => CurrentValue == MinValue;
        public bool IsMax => CurrentValue == MaxValue;
    }
}