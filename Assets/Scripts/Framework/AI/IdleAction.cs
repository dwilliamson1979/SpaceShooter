using UnityEngine;

namespace com.dhcc.framework.ai
{
    [CreateAssetMenu(fileName = "IdleAction", menuName = "UtilityAI/Actions/IdleAction")]
    public class IdleAction : AIAction
    {
        public override void Execute(Context context)
        {
            context.agent.SetDestination(context.agent.transform.position);
        }
    }
}