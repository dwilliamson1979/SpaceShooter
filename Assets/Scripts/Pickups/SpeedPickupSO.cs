using com.dhcc.components;
using com.dhcc.utility;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedPickupSO", menuName = "Pickups/SpeedPickupSO")]
public class SpeedPickupSO : PickupSO
{
    [Header("Settings")]
    [SerializeField] private float duration;
    [SerializeField] private float speedMultiplier;

    public override void TryToPickup(Pickup pickup, GameObject obj)
    {
        //TODO Need to develop a system for extending a pickup. Ex. If the speed pickup is in effect and it is picked up again, it should add more time.
        var moveComp = obj.GetComponent<MovementComp>();
        if (moveComp != null)
        {
            moveComp.SetSpeed(moveComp.DefaultSpeed  * (1 + speedMultiplier));
            AudioManager.Instance.PlaySoundFx(pickupSound);
            TimerManager.SetTimer(() =>
            {
                if (moveComp != null)
                {
                    moveComp.SetSpeed(moveComp.DefaultSpeed);
                }
            }, duration);
            pickup.PickupComplete();
        }
    }
}