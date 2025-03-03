using UnityEngine;

namespace com.dhcc.framework.ai
{
    [CreateAssetMenu(fileName = "ConstantConsiderationSO", menuName = "UtilityAI/Considerations/Constant")]
    public class ConstantConsideration : Consideration
    {
        public float Value;

        public override float Evaluate(Context context) => Value;
    }
}
