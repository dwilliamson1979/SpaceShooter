using System;
using UnityEngine;
using com.dhcc.utility;

namespace com.dhcc.core
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
        [field: SerializeField, ReadOnly] public int CurrentValue { get; private set; }

        public float Percentage => CurrentValue / (float)MaxValue;

        public void SetMin(int value) => MinValue = value;
        public void SetMax(int value) => MaxValue = value;

        public void SetValue(int value) => CurrentValue = Mathf.Clamp(value, MinValue, MaxValue);
    }
}