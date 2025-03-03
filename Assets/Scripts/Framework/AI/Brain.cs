using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace com.dhcc.framework.ai
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Sensor))]
    public class Brain : MonoBehaviour
    {
        public List<AIAction> actions;
        public Context context;

        private void Awake()
        {
            context = new(this);

            foreach (var action in actions)
                action.Init(context);
        }

        private void Update()
        {
            //Currently doing this in update. Consider a timer to decrease load on the system. I also wonder if running on update will create indecisive (knee-jerk) brains.

            UpdateContext();

            AIAction bestAction = null;
            float bestUtilityScore = float.MinValue;
            for (int i = 0; i < actions.Count; i++)
            {
                float utilityScore = actions[i].CalculateUtility(context);
                if (bestUtilityScore < utilityScore)
                {
                    bestAction = actions[i];
                    bestUtilityScore = utilityScore;
                }
            }

            bestAction?.Execute(context);
        }

        private void UpdateContext()
        {
            //Gather the most up-to-date information about the world around us.
        }
    }
}