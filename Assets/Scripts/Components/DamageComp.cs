using com.dhcc.core;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.components
{
    public class DamageComp : MonoBehaviour
    {
        private List<IDamageReceiver> damageReceivers = new();

        public void Register(IDamageReceiver damageReceiver)
        {
            damageReceivers.Add(damageReceiver);
            damageReceivers.Sort((x, y) => -x.DamagePriority.CompareTo(y.DamagePriority));
        }

        public void Deregister(IDamageReceiver damageReceiver)
        {
            damageReceivers.Remove(damageReceiver);
            damageReceivers.Sort((x, y) => -x.DamagePriority.CompareTo(y.DamagePriority));
        }

        public int TakeDamage(EDamageType damageType, int amount)
        {
            //NOTE Old comment. Not sure if it is still relevant.
            //Sword, fire, heal
            //What if a spell makes a person sickly and any attempts to heal cause damage? Maybe the Sickly IV spell creates an IDamageReceiver with a high priority and registers it.
            //This IDamageReceived converts EDamageType::Health to EDamageType::BasicDamage and ensure the number is positive which inflicts damage. It then passes it on to the next
            //IDamageReceiver. But, if we want it to bypass the shield and hurt the players health we will need to convert it to something like EDamageType::Sickly?

            int damageTaken = 0;

            for (int i = 0; i < damageReceivers.Count; i++)
            {
                damageTaken += Mathf.Abs(damageReceivers[i].TakeDamage(damageType, amount));

                amount -= damageTaken;

                if (amount < 0) break;

                //Ex.float based system
                //if (Mathf.Approximately(amount, 0)) break;
            }

            return damageTaken;
        }
    }
}