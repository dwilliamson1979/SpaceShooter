using UnityEngine;

public class Shield : Powerup
{
    protected override void Pickup(Player player)
    {
        player.ActivateShield();
    }
}