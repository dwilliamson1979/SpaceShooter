using com.dhcc.framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.dhcc.spaceshooter
{
    public class Player : MonoBehaviour, IDamageable
    {
        [Serializable]
        struct DamageLocation
        {
            public GameObject Location;
            public GameObject DamageVisual;
        }

        [Header("Settings")]
        [SerializeField] private Boundary boundary;
        [SerializeField] private bool wrapHorizontalMovement;
        [SerializeField] private Vector2 horizontalWrapBounds;
        [SerializeField] private float fireRate;
        [SerializeField] private LayerMask projectileLayer;
        [SerializeField] private bool isGodModeOn;

        [SerializeField] private int startingAmmo;
        [SerializeField] private int startinghealth;
        [SerializeField] private int startingShield;

        [Header("References")]
        [SerializeField] private Transform primaryMuzzlePoint;
        [SerializeField] private Transform leftMuzzlePoint;
        [SerializeField] private Transform rightMuzzlePoint;
        [SerializeField] private GameObject damageEffect;
        [SerializeField] private GameObject shieldSprite;
        [SerializeField] private AudioClip laserAudio;
        [SerializeField] private DamageLocation[] damagePoints;

        private float nextAllowedFireTime;        

        private bool hasTripleShot;
        private AsyncTimer tripleShotTimer;
        private bool hasAngleShot;
        private AsyncTimer angleShotTimer;
        private bool hasHomingShot;

        private int currentAmmo;

        private List<GameObject> damageInstances = new();

        private InputComp inputComp;
        private DamageComp damageComp;
        private HealthComp healthComp;
        private ShieldComp shieldComp;
        private MovementComp moveComp;
        private ThrusterComp thrusterComp;        

        private void Awake()
        {
            inputComp = GetComponent<InputComp>();
            damageComp = GetComponent<DamageComp>();
            healthComp = GetComponent<HealthComp>();
            shieldComp = GetComponent<ShieldComp>();
            moveComp = GetComponent<MovementComp>();
            thrusterComp = GetComponent<ThrusterComp>();
        }

        void Start()
        {
            transform.position = Vector3.zero;
            AddAmmo(startingAmmo);

            healthComp.OnHealthChanged += OnHealthChanged;
            healthComp.TakeDamage(EDamageType.Health, startinghealth);

            shieldComp.OnShieldChanged += OnShieldChanged;
            shieldComp.TakeDamage(EDamageType.Shield, startingShield);

            moveComp.SetSpeed(moveComp.DefaultSpeed);

            thrusterComp.OnThrustChanged += OnThrustChanged;
            thrusterComp.OnFuelChanged += OnFuelChanged;

            inputComp.OnSprintInput += OnThrustInput;

            angleShotTimer = new(() => hasAngleShot = false, 5f);
            tripleShotTimer = new(() => hasTripleShot = false, 5f);
        }

        private void OnThrustInput(bool thrust)
        {
            thrusterComp.Thrust(thrust);
        }

        private void OnThrustChanged()
        {
            //TODO Start/stop a visual effect? Move this back into the ThrusterComp?
            //UIManager.Instance.UpdateThruster(thrusterComp.Fuel.Percentage);

            if (thrusterComp.IsThrusting)
                moveComp.SetSpeed(moveComp.DefaultSpeed * (1f + thrusterComp.SpeedModifier));
            else
                moveComp.SetSpeed(moveComp.DefaultSpeed);
        }

        private void OnFuelChanged()
        {
            UIManager.Instance.UpdateThruster(thrusterComp.Fuel.Percentage);
        }

        void Update()
        {
            ProcessFiring();

            if (Input.GetKeyDown(KeyCode.C))
                GameEvents.AutomaticPickupCheat.Raise(transform);
        }

        private void LateUpdate()
        {
            //TODO When should movement be processed? Does it depend on whether the movement is usign manual translation or physics?
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            moveComp.Move(inputComp.MoveInput);

            if (wrapHorizontalMovement)
            {
                float x = transform.position.x;
                if (x < horizontalWrapBounds.x)
                    x = horizontalWrapBounds.y;
                else if (x > horizontalWrapBounds.y)
                    x = horizontalWrapBounds.x;

                transform.position = new Vector3(
                    x,
                    Mathf.Clamp(transform.position.y, boundary.Bottom, boundary.Top),
                    0.0f);
            }
            else
            {
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, boundary.Left, boundary.Right),
                    Mathf.Clamp(transform.position.y, boundary.Bottom, boundary.Top),
                    0.0f);
            }
        }

        private void ProcessFiring()
        {
            if (currentAmmo <= 0 || Time.time < nextAllowedFireTime || !Input.GetButton("Fire1")) return;

            nextAllowedFireTime = Time.time + fireRate;

            if (hasAngleShot)
            {
                var laser1 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser1.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser1.SetLayerMask(projectileLayer);
                var laser2 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser2.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation * Quaternion.Euler(0f, 0f, 45f));
                laser2.SetLayerMask(projectileLayer);
                var laser3 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser3.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation * Quaternion.Euler(0f, 0f, -45f));
                laser3.SetLayerMask(projectileLayer);
            }
            else if (hasTripleShot)
            {
                var laser1 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser1.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser1.SetLayerMask(projectileLayer);
                var laser2 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser2.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation);
                laser2.SetLayerMask(projectileLayer);
                var laser3 = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser3.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation);
                laser3.SetLayerMask(projectileLayer);
            }
            else if (hasHomingShot)
            {
                var homingMissile = PoolManager.Get<HomingMissle>(EPoolIdentifier.HomingMissile);
                homingMissile.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                homingMissile.SetLayerMask(projectileLayer);
                hasHomingShot = false;
            }
            else
            {
                var laser = PoolManager.Get<Projectile>(EPoolIdentifier.Laser);
                laser.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser.SetLayerMask(projectileLayer);
            }

            AddAmmo(-1);

            AudioManager.Instance.PlaySoundFx(laserAudio);
        }

        public int TakeDamage(EDamageType damageType, int amount)
        {
            return isGodModeOn ? 0 : damageComp.TakeDamage(damageType, amount);
        }

        private void OnHealthChanged(int delta, HealthComp healthComp)
        {
            if (delta != 0)
                GameEvents.PlayerHealthChanged.Raise(delta, healthComp.Health.CurrentValue);

            if (delta > 0)
            {
                //Fix a random damage visual.
                List<int> usableIndexes = new();
                for (int i = 0; i < damagePoints.Length; i++)
                {
                    if (damagePoints[i].Location.activeSelf)
                        usableIndexes.Add(i);
                }

                if (usableIndexes.Count > 0)
                {
                    int selectedIndex = usableIndexes[UnityEngine.Random.Range(0, usableIndexes.Count)];
                    damagePoints[selectedIndex].Location.SetActive(false);
                    GameObject temp = damagePoints[selectedIndex].DamageVisual;
                    damagePoints[selectedIndex].DamageVisual = null;
                    Destroy(temp);
                }
            }
            else
            {
                //Damage
                //Pick a random damage transform and instantiate a damage prefab. Setting to active lets us know it is in use.
                List<int> usableIndexes = new();
                for (int i = 0; i < damagePoints.Length; i++)
                {
                    if (!damagePoints[i].Location.activeSelf)
                        usableIndexes.Add(i);
                }

                int selectedIndex = usableIndexes[UnityEngine.Random.Range(0, usableIndexes.Count)];
                damagePoints[selectedIndex].Location.SetActive(true);

                GameObject damageInstance = Instantiate(damageEffect, damagePoints[selectedIndex].Location.transform.position, damagePoints[selectedIndex].Location.transform.rotation, transform);
                damagePoints[selectedIndex].DamageVisual = damageInstance;
            }

            if (healthComp.Health.CurrentValue <= 0)
                Die();
        }

        private void Die()
        {
            GameEvents.GameOver.Raise();

            Destroy(gameObject);
        }

        private void OnShieldChanged(int delta, ShieldComp shieldComp)
        {
            if (shieldComp.Shield.CurrentValue > 0)
            {
                shieldSprite.SetActive(true);

                var sr = shieldSprite.GetComponent<SpriteRenderer>();
                Color color = sr.color;
                color.a = shieldComp.Shield.Percentage;
                sr.color = color;
            }
            else
                shieldSprite.SetActive(false);
        }

        public void ActivateTripleShot()
        {
            hasHomingShot = false;
            hasAngleShot = false;
            angleShotTimer.Stop();

            hasTripleShot = true;
            tripleShotTimer.Start();
        }

        public void ActivateAngleShot()
        {
            hasHomingShot = false;
            hasTripleShot = false;
            tripleShotTimer.Stop();

            hasAngleShot = true;
            angleShotTimer.Start();
        }

        public void ActivateHomingMissile()
        {
            hasAngleShot = false;
            angleShotTimer.Stop();
            hasTripleShot = false;
            tripleShotTimer.Stop();

            hasHomingShot = true;
        }

        public int AddAmmo(int amount)
        {
            int previousAmmo = currentAmmo;
            currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, startingAmmo);
            UIManager.Instance.UpdateAmmo(currentAmmo);
            return currentAmmo - previousAmmo;
        }
    }
}