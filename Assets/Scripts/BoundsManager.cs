using com.dhcc.pool;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    public static BoundsManager Instance;

    [Header("Settings")]
    [field: SerializeField] public Vector2 HorizontalBoundary { get; private set; }
    [field: SerializeField] public Vector2 VerticalBoundary { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool IsOutOfBounds(Transform transform)
    {
        return transform.position.x < HorizontalBoundary.x
            || transform.position.x > HorizontalBoundary.y
            || transform.position.y < VerticalBoundary.x
            || transform.position.y > HorizontalBoundary.y;
    }
}