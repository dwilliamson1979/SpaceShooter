using com.dhcc.framework;
using System;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Pickup : MonoBehaviour, IPoolObject
    {
        [Header("Settings")]
        [SerializeField] private float speed;

        [Header("References")]
        [SerializeField] private PickupSO pickupSO;

        public event System.Action PoolRelease;

        private bool isAutoPickup;
        private Transform playerTransform;

        private void OnEnable()
        {
            GameEvents.AutomaticPickupCheat.Subscribe(HandleAutomaticPickupCheat);
        }        

        private void OnDisable()
        {
            GameEvents.AutomaticPickupCheat.Unsubscribe(HandleAutomaticPickupCheat);
        }

        void Start()
        {
            SetSpawnPosition();
        }

        void Update()
        {
            if (isAutoPickup)
            {
                Vector3 moveDirection = playerTransform.position - transform.position;
                moveDirection.Normalize();
                transform.Translate(Time.deltaTime * (speed * 4f) * moveDirection);
            }
            else
                transform.Translate(Time.deltaTime * speed * -Vector3.up);

            if (BoundsManager.Instance.IsOutOfBottomBounds(transform.position.y))
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
            isAutoPickup = false;
            gameObject.SetActive(false);
            PoolRelease?.Invoke();
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

        private void HandleAutomaticPickupCheat(Transform transform)
        {
            isAutoPickup = true;
            playerTransform = transform;
        }
    }
}