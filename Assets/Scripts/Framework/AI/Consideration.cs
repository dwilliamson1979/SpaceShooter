using UnityEngine;

namespace com.dhcc.framework.ai
{
    public abstract class Consideration : ScriptableObject
    {
        public abstract float Evaluate(Context context);
    }
}