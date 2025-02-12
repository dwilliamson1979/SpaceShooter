using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int lives;
    [SerializeField] private float speed;
    [SerializeField] private bool wrapHorizontalMovement;
    [SerializeField] private Vector2 verticalBounds;
    [SerializeField] private Vector2 horizontalBounds;
    [SerializeField] private Vector2 horizontalWrapBounds;
    [SerializeField] private float fireRate;
    [SerializeField] private float speedBoostModifier;
    [SerializeField] private LayerMask projectileLayer;
    [SerializeField] private int defaultShieldHealth;

    [Header("References")]
    [SerializeField] private Projectile laserPrefab;
    [SerializeField] private Transform primaryMuzzlePoint;
    [SerializeField] private Transform leftMuzzlePoint;
    [SerializeField] private Transform rightMuzzlePoint;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject shieldSprite;
    [SerializeField] private AudioClip laserAudio;
    [SerializeField] private GameObject[] damagePoints;

    private float nextAllowedFireTime;
    private int score;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 direction;

    private bool hasTripleShot;
    Coroutine tripleShotRoutine;
    
    private bool hasSpeedBoost;
    Coroutine speedBoostRoutine;
    private float currentSpeed;
    
    private bool hasShield;
    private int shieldHealth;

    private List<GameObject> damageInstances = new();

    void Start()
    {
        transform.position = Vector3.zero;
        currentSpeed = speed;
        UIManager.Instance.UpdateLives(lives);
    }

    void Update()
    {
        ProcessMovement();
        ProcessFiring();
    }

    private void ProcessMovement()
    {
        //This method only captures the last axis. It will not capture both left and right at the same time (which creates a stalemate).
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(Time.deltaTime * currentSpeed * direction);

        //if (Input.GetKey(KeyCode.D))
        //    transform.Translate(Vector3.right * Time.deltaTime * speed);
        //if (Input.GetKey(KeyCode.A))
        //    transform.Translate(-Vector3.right * Time.deltaTime * speed);
        //if (Input.GetKey(KeyCode.W))
        //    transform.Translate(Vector3.up * Time.deltaTime * speed);
        //if (Input.GetKey(KeyCode.S))
        //    transform.Translate(-Vector3.up * Time.deltaTime * speed);

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
        if (Time.time < nextAllowedFireTime || !Input.GetButton("Fire1")) return;

        nextAllowedFireTime = Time.time + fireRate;

        if(hasTripleShot)
        {
            if (laserPrefab != null)
            {
                var laser1 = LaserPool.Get();// Instantiate<Laser>(laserPrefab, primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser1.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser1.SetLayerMask(projectileLayer);
                var laser2 = LaserPool.Get();// Instantiate(laserPrefab, leftMuzzlePoint.position, leftMuzzlePoint.rotation);
                laser2.transform.SetPositionAndRotation(leftMuzzlePoint.position, leftMuzzlePoint.rotation);
                laser2.SetLayerMask(projectileLayer);
                var laser3 = LaserPool.Get();// Instantiate(laserPrefab, rightMuzzlePoint.position, rightMuzzlePoint.rotation);
                laser3.transform.SetPositionAndRotation(rightMuzzlePoint.position, rightMuzzlePoint.rotation);
                laser3.SetLayerMask(projectileLayer);
            }
        }
        else
        {
            if (laserPrefab != null)
            {
                var laser = LaserPool.Get();// Instantiate(laserPrefab, primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser.transform.SetPositionAndRotation(primaryMuzzlePoint.position, primaryMuzzlePoint.rotation);
                laser.SetLayerMask(projectileLayer);
            }
        }

        AudioManager.Instance.PlaySoundFx(laserAudio);
    }

    public void Damage()
    {
        if(hasShield)
        {
            DamageShield(1);
            return;
        }

        lives--;

        if (lives > 0)
            AddDamageVisual(); 

        UIManager.Instance.UpdateLives(lives);

        if (lives <= 0)
            Die();
    }

    private void AddDamageVisual()
    {
        var pointsArray = damagePoints.Where(go => !go.activeSelf).ToArray();
        var damagePoint = pointsArray[Random.Range(0, pointsArray.Length)];
        damagePoint.SetActive(true);

        var damageInstance = Instantiate(damageEffect, damagePoint.transform.position, damagePoint.transform.rotation, transform);
        damageInstances.Add(damageInstance);
    }

    private void Die()
    {
        UIManager.Instance.GameOver();
        SpawnManager.Instance.StopSpawning();
        GameManager.Instance.GameOver();
        Destroy(gameObject);
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

    public void ActivateShield()
    {
        DamageShield(-defaultShieldHealth);
        shieldSprite.SetActive(true);
    }

    private void DamageShield(int damage)
    {
        shieldHealth -= damage;
        hasShield = shieldHealth > 0;
        shieldSprite.SetActive(hasShield);

        var sr = shieldSprite.GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = shieldHealth / (float)defaultShieldHealth;
        sr.color = color;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }

    public void AddLife()
    {
        if (lives >= 3) return;

        lives++;
        UIManager.Instance.UpdateLives(lives);

        damagePoints.First(go => go.activeSelf).SetActive(false);

        if (damageInstances.Count > 0)
        {
            int index = Random.Range(0, damageInstances.Count);
            var damageInstance = damageInstances[index];
            damageInstances.RemoveAt(index);
            Destroy(damageInstance);
        }
    }
}