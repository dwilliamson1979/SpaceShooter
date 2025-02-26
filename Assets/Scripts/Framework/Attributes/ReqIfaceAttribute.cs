using System;
using UnityEngine;

namespace com.dhcc.framework
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ReqIfaceAttribute : PropertyAttribute
    {
        public Type InterfaceType;

        public ReqIfaceAttribute(Type interfaceType) => InterfaceType = interfaceType;
    }
}