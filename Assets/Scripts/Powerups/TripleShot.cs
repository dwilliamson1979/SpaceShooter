using com.dhcc.pool;
using UnityEngine;

public class TripleShot : Pickup
{
    protected override void TryToPickup(GameObject obj)
    {
        var player = obj.GetComponent<Player>();
        if (player != null)
            player.ActivateTripleShot();
    }
}