using UnityEngine;

public class FlowerEnemy : MonoBehaviour
{   
    private GameObject player;
    private Transform playerTransform;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float range = 5f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float projectileSpeed = 5f;

    private float fireCountdown = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        if(player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= range && fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = fireRate;
        }

        if (fireCountdown > 0f)
        {
            // Decrease the countdown timer
            fireCountdown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set projectile's gravity and velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, 5f); // Destroy the projectile after 5 seconds to prevent clutter
    }
}
