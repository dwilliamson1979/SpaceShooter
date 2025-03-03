using UnityEngine;

namespace com.dhcc.framework.ai
{
    public abstract class AIAction : ScriptableObject
    {
        public string targetTag;
        public Consideration consideration;

        public virtual void Init(Context context)
        {
            //Optional
        }

        public float CalculateUtility(Context context) => consideration.Evaluate(context);

        public abstract void Execute(Context context);
    }
}