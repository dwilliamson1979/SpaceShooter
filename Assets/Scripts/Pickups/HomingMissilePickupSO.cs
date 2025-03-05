using UnityEngine;

namespace com.dhcc.spaceshooter
{
    [CreateAssetMenu(fileName = "HomingMissilePickupSO", menuName = "Pickups/HomingMissilePickupSO")]
    public class HomingMissilePickupSO : PickupSO
    {
        //[Header("Settings")]
        //[SerializeField] private float duration;

        public override void TryToPickup(Pickup pickup, GameObject obj)
        {
            //TODO Need to develop a system for extending a pickup. Ex. If the speed pickup is in effect and it is picked up again, it should add more time.
            var player = obj.GetComponent<Player>();
            if (player != null)
            {
                player.ActivateHomingMissile();
                AudioManager.Instance.PlaySoundFx(pickupSound);
                pickup.PickupComplete();
            }
        }
    }
}