
namespace com.dhcc.spaceshooter
{
    public interface IDamageReceiver
    {
        int DamagePriority { get; }
        int TakeDamage(EDamageType damageType, int amount);
    }
}