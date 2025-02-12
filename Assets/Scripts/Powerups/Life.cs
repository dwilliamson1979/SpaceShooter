using UnityEngine;

public class Life : Powerup
{
    [SerializeField] private int livesToGive;

    protected override void Pickup(Player player)
    {
        player.Heal(livesToGive);
    }
}