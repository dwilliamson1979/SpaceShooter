using UnityEngine;

public class Drain : Powerup
{
    [SerializeField] private int livesToTake;

    protected override void Pickup(Player player)
    {
        player.Damage(livesToTake);
    }
}