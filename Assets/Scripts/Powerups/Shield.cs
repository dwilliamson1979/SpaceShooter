using com.dhcc.core;
using UnityEngine;

public class Shield : Powerup
{
    [SerializeField] private int shieldToGive;
    [SerializeField] private EDamageType damageType;

    protected override void Pickup(Player player)
    {
        player.TakeDamage(EDamageType.Shield, shieldToGive);
    }
}