using System;
using UnityEngine;
using com.dhcc.utility;

namespace com.dhcc.core
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
        [field: SerializeField, ReadOnly] public float CurrentValue { get; private set; }

        public float Percentage => CurrentValue / MaxValue;
        public bool IsAtMin => Mathf.Abs(CurrentValue - MinValue) < 0.00001f;
        public bool IsAtMax => Mathf.Abs(CurrentValue - MaxValue) < 0.00001f;

        public void SetMin(float value) => MinValue = value;
        public void SetMax(float value) => MaxValue = value;

        public void SetValue(float value) => CurrentValue = Mathf.Clamp(value, MinValue, MaxValue);
        public void Add(float value) => CurrentValue = Mathf.Clamp(CurrentValue + value, MinValue, MaxValue);
    }
}