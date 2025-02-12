using UnityEngine;

public class Life : Powerup
{
    protected override void Pickup(Player player)
    {
        player.AddLife();
    }
}