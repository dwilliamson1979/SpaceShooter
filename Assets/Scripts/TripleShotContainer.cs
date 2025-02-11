using UnityEngine;

public class TripleShotContainer : MonoBehaviour
{
    void Start()
    {
        //NOTE This would not work properly with pooling. Ex. The triple shot would go back to the pool while each individual laser may not.
        //What if one laser hit something while the other did not. One goes to the pool, the other does not. Pool status is not synchronized.
        transform.DetachChildren();
        Destroy(gameObject);
    }
}