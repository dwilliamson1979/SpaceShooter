using com.dhcc.core;
using UnityEngine;

public class Drain : Powerup
{
    [SerializeField] private int lifeToTake;
    [SerializeField] private EDamageType damageType;

    protected override void Pickup(Player player)
    {
        player.TakeDamage(damageType, lifeToTake);
    }
}