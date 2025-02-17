using System;
using System.Collections;
using System.Collections.Generic;
using com.dhcc.core;
using com.dhcc.components;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Serializable]
    struct DamageLocation
    {
        public GameObject Location;
        public GameObject DamageVisual;
    }

    [Header("Settings")]    
    [SerializeField] private float speed;
    [SerializeField] private bool wrapHorizontalMovement;
    [SerializeField] private Vector2 verticalBounds;
    [SerializeField] private Vector2 horizontalBounds;
    [SerializeField] private Vector2 horizontalWrapBounds;
    [SerializeField] private float fireRate;
    [SerializeField] private float speedBoostModifier;
    [SerializeField] private LayerMask projectileLayer;
    
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
    private int score;

    private bool hasTripleShot;
    Coroutine tripleShotRoutine;
    
    private bool hasSpeedBoost;
    Coroutine speedBoostRoutine;
    private float currentSpeed;

    private int currentAmmo;

    private List<GameObject> damageInstances = new();

    private PlayerInput playerInput;
    private DamageComp damageComp;
    private HealthComp healthComp;
    private ShieldComp shieldComp;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        damageComp = GetComponent<DamageComp>();
        healthComp = GetComponent<HealthComp>();
        shieldComp = GetComponent<ShieldComp>();
    }

    void Start()
    {
        transform.position = Vector3.zero;
        currentSpeed = speed;
        AddAmmo(startingAmmo);

        healthComp.OnHealthChanged += OnHealthChanged;
        healthComp.TakeDamage(EDamageType.Health, startinghealth);

        shieldComp.OnShieldChanged += OnShieldChanged;
        shieldComp.TakeDamage(EDamageType.Shield, startingShield);
    }

    void Update()
    {
        ProcessMovement();
        ProcessFiring();
    }

    private void LateUpdate()
    {
        //TODO When should movement be processed? Does it depend on whether the movement is usign manual translation or physics?
        //ProcessMovement();
    }

    private void ProcessMovement()
    {
        ////This method only captures the last axis. It will not capture both left and right at the same time (which creates a stalemate).
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        //direction = new Vector3(horizontalInput, verticalInput, 0);
        //transform.Translate(Time.deltaTime * currentSpeed * direction);

        ////if (Input.GetKey(KeyCode.D))
        ////    transform.Translate(Vector3.right * Time.deltaTime * speed);
        ////if (Input.GetKey(KeyCode.A))
        ////    transform.Translate(-Vector3.right * Time.deltaTime * speed);
        ////if (Input.GetKey(KeyCode.W))
        ////    transform.Translate(Vector3.up * Time.deltaTime * speed);
        ////if (Input.GetKey(KeyCode.S))
        ////    transform.Translate(-Vector3.up * Time.deltaTime * speed);

        transform.Translate(Time.deltaTime * currentSpeed * playerInput.MoveVector);

        if (!wrapHorizontalMovement)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, horizontalBounds.x, horizontalBounds.y),
                transform.position.y,
                0.0f);
        }
        else
        {
            if (transform.position.x < horizontalWrapBounds.x)
                transform.position = new Vector3(horizontalWrapBounds.y, transform.position.y);
            else if (transform.position.x > horizontalWrapBounds.y)
                transform.position = new Vector3(horizontalWrapBounds.x, transform.position.y);
        }

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, verticalBounds.x, verticalBounds.y),
            0.0f);
    }

    private void ProcessFiring()
    {
        if (currentAmmo <= 0 || Time.time < nextAllowedFireTime || !Input.GetButton("Fire1")) return;

        nextAllowedFireTime = Time.time + fireRate;

        if(hasTripleShot)
        {
            var laser1 = LaserPool.Get();
            laser1.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
            laser1.SetLayerMask(projectileLayer);
            var laser2 = LaserPool.Get();
            laser2.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation);
            laser2.SetLayerMask(projectileLayer);
            var laser3 = LaserPool.Get();
            laser3.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation);
            laser3.SetLayerMask(projectileLayer);
        }
        else
        {
            var laser = LaserPool.Get();
            laser.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
            laser.SetLayerMask(projectileLayer);
        }

        AddAmmo(-1);

        AudioManager.Instance.PlaySoundFx(laserAudio);
    }

    public int TakeDamage(EDamageType damageType, int amount)
    {
        return damageComp.TakeDamage(damageType, amount);
    }

    private void OnHealthChanged(int delta, HealthComp healthComp)
    {
        UIManager.Instance.UpdateLives(healthComp.Health.CurrentValue);

        if (delta < 0)
        {//Fix a random damage visual.
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
        UIManager.Instance.GameOver();
        SpawnManager.Instance.StopSpawning();
        GameManager.Instance.GameOver();
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
        hasTripleShot = true;

        if(tripleShotRoutine != null)
            StopCoroutine(tripleShotRoutine);

        tripleShotRoutine = StartCoroutine(TripleShotStopRoutine());
    }

    private IEnumerator TripleShotStopRoutine()
    {
        yield return new WaitForSeconds(5f);
        hasTripleShot = false;
    }

    public void ActivateSpeedBoost()
    {
        hasSpeedBoost = true;
        currentSpeed = speedBoostModifier * speed + speed;

        if (speedBoostRoutine != null)
            StopCoroutine(speedBoostRoutine);

        speedBoostRoutine = StartCoroutine(SpeedBoostRoutine());
    }

    private IEnumerator SpeedBoostRoutine()
    {
        yield return new WaitForSeconds(5f);
        hasSpeedBoost = false;
        currentSpeed = speed;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, startingAmmo);
        UIManager.Instance.UpdateAmmo(currentAmmo);
    }
}