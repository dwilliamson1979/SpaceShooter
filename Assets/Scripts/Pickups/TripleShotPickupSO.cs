using com.dhcc.components;
using com.dhcc.utility;
using UnityEngine;

[CreateAssetMenu(fileName = "TripleShotPickupSO", menuName = "Pickups/TripleShotPickupSO")]
public class TripleShotPickupSO : PickupSO
{
    [Header("Settings")]
    [SerializeField] private float duration;

    public override void TryToPickup(Pickup pickup, GameObject obj)
    {
        //TODO Need to develop a system for extending a pickup. Ex. If the speed pickup is in effect and it is picked up again, it should add more time.
        var player = obj.GetComponent<Player>();
        if (player != null)
        {
            player.ActivateTripleShot();
            AudioManager.Instance.PlaySoundFx(pickupSound);
            TimerManager.SetTimer(() =>
            {
                if (player != null)
                    player.DeactivateTripleShot();
            }
            , duration);
            pickup.PickupComplete();
        }
    }
}