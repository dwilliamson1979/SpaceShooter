using com.dhcc.core;
using UnityEngine;

public class Life : Powerup
{
    [SerializeField] private int lifeToGive;
    [SerializeField] private EDamageType damageType;

    protected override void Pickup(Player player)
    {
        player.TakeDamage(damageType, lifeToGive);
    }
}