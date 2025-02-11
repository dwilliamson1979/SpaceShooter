using System.Collections;
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

    [Header("References")]
    [SerializeField] private Transform laserPrefab;
    [SerializeField] private Transform tripleLaserPrefab;
    [SerializeField] private Transform muzzlePoint;
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
            if (tripleLaserPrefab != null)
                Instantiate(tripleLaserPrefab, muzzlePoint.position, muzzlePoint.rotation);
        }
        else
        {
            if (laserPrefab != null)
                Instantiate(laserPrefab, muzzlePoint.position, muzzlePoint.rotation);
        }

        AudioManager.Instance.PlaySoundFx(laserAudio);
    }

    public void Damage()
    {
        if(hasShield)
        {
            DeactivateShield();
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

        Instantiate(damageEffect, damagePoint.transform.position, damagePoint.transform.rotation, transform);
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
        hasShield = true;
        shieldSprite.SetActive(true);
    }

    private void DeactivateShield()
    {
        hasShield = false;
        shieldSprite.SetActive(false);
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }
}