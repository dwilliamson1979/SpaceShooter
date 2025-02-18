using com.dhcc.core;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPickupSO", menuName = "Pickups/AmmoPickupSO")]
public class AmmoPickupSO : PickupSO
{
    [Header("Settings")]
    [SerializeField] private int amount;
    //[SerializeField] private EAmmoType ammoType;

    public override void TryToPickup(Pickup pickup, GameObject obj)
    {
        var player = obj.GetComponent<Player>();
        if (player != null)
        {
            if (player.AddAmmo(amount) != 0)
            {
                AudioManager.Instance.PlaySoundFx(pickupSound);
                pickup.PickupComplete();
            }
        }
    }
}