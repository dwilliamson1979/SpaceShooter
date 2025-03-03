using com.dhcc.framework;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Pickup : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private float speed;

        [Header("References")]
        [SerializeField] private PickupSO pickupSO;

        public event System.Action ReleaseToPool;

        void Start()
        {
            SetSpawnPosition();
        }

        void Update()
        {
            transform.Translate(Time.deltaTime * speed * -Vector3.up);

            if (transform.position.y < BoundsManager.Instance.VerticalBoundary.x)
                SetSpawnPosition();
        }

        private void SetSpawnPosition()
        {
            Vector3 spawnPosition = SpawnManager.Instance.GetSpawnPoint();
            spawnPosition.z = transform.position.z;
            transform.position = spawnPosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (pickupSO != null)
                    pickupSO.TryToPickup(this, other.gameObject);
            }
        }

        public virtual void PickupComplete()
        {
            gameObject.SetActive(false);
            ReleaseToPool?.Invoke();
        }

        public void PoolOnCreate()
        {

        }

        public void PoolOnGet()
        {
            //TODO Do we need to reposition the object offscreen to prevent the possibility of it being reactviated at its last know position (possibly onscreen)?
            gameObject.SetActive(true);
        }

        public void PoolOnRelease()
        {
            gameObject.SetActive(false);
        }

        public void PoolOnDestroy()
        {

        }
    }
}