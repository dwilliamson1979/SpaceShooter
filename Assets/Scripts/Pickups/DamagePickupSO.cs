using UnityEngine;

namespace com.dhcc.spaceshooter
{
    [CreateAssetMenu(fileName = "DamagePickupSO", menuName = "Pickups/DamagePickupSO")]
    public class DamagePickupSO : PickupSO
    {
        [Header("Settings")]
        [SerializeField] private int amount;
        [SerializeField] private EDamageType damageType;

        public override void TryToPickup(Pickup pickup, GameObject obj)
        {
            var player = obj.GetComponent<Player>();
            if (player != null)
            {
                if (player.TakeDamage(damageType, amount) != 0)
                {
                    AudioManager.Instance.PlaySoundFx(pickupSound);
                    pickup.PickupComplete();
                }
            }
        }
    }
}