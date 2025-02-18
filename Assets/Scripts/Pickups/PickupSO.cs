using UnityEngine;

public abstract class PickupSO : ScriptableObject
{
    [Header("References")]
    [SerializeField] protected AudioClip pickupSound;

    public abstract void TryToPickup(Pickup pickup, GameObject obj);
}