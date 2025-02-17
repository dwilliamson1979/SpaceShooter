using System;
using UnityEngine;

namespace com.dhcc.core
{
    public interface IDamageReceiver
    {
        int DamagePriority { get; }
        int TakeDamage(EDamageType damageType, int amount);
    }
}