using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    
    protected GameObject player;
    protected Transform playerTransform;
    protected const string PLAYER = "Player";

    [SerializeField] protected GameObject projectilePrefab;

    [SerializeField] protected float projectileSpeed = 5f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 2f;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag(PLAYER);
        
        if(player != null)
        {
            playerTransform = player.transform;
        }
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void ShootAtPlayer()
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
