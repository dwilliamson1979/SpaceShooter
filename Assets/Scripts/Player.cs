using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int speed;

    [SerializeField] private bool wrapHorizontalMovement;
    [SerializeField] private Vector2 verticalBounds;
    [SerializeField] private Vector2 horizontalBounds;
    [SerializeField] private Vector2 horizontalWrapBounds;

    [SerializeField] private Transform laserPrefab;
    [SerializeField] private Transform muzzlePoint;

    [SerializeField] private float fireRate;
    private float nextAllowedFireTime;

    void Start()
    {
        transform.position = Vector3.zero;
    }

    void Update()
    {
        ProcessMovement();
        ProcessFiring();
    }

    private void ProcessMovement()
    {
        //This method only captures the last axis. It will not capture both left and right at the same time (which creates a stalemate).
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * Time.deltaTime * speed);

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
        if (Time.time < nextAllowedFireTime) return;

        if (!Input.GetButton("Fire1")) return;

        nextAllowedFireTime = Time.time + fireRate;

        if (laserPrefab == null) return;
        Instantiate(laserPrefab, muzzlePoint.position, Quaternion.identity);
    }
}