using UnityEngine;

public class SpeedBoost : Pickup
{
    protected override void TryToPickup(GameObject obj)
    {
        var player = obj.GetComponent<Player>();
        if (player != null)
            player.ActivateSpeedBoost();
    }
}
