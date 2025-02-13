using UnityEngine;

public class Ammo : Powerup
{
    [SerializeField] private int amount;

    protected override void Pickup(Player player)
    {
        player.AddAmmo(amount);
    }
}