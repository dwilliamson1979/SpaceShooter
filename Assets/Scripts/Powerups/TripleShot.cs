using com.dhcc.pool;
using UnityEngine;

public class TripleShot : Powerup
{
    protected override void Pickup(Player player)
    {
        player.ActivateTripleShot();
    }
}