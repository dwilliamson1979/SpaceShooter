using UnityEngine;

public static class GameObjectExts
{
    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
    {
        T comp = gameObject.GetComponent<T>();
        if (comp != null) return comp;

        return gameObject.AddComponent<T>();
    }
}