using System;
using UnityEngine;

namespace com.dhcc.utility
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}