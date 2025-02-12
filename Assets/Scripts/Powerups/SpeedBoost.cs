using UnityEngine;

public class SpeedBoost : Powerup
{
    protected override void Pickup(Player player)
    {
        player.ActivateSpeedBoost();
    }
}
