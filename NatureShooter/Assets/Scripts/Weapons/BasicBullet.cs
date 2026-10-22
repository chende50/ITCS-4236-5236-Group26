using UnityEngine;

public class BasicBullet : MonoBehaviour
{

	private Vector3 direction;
	private float projectileSpeed;
	private float range;
	private float damage;

	private Vector3 startPosition;

	public void Constructor(Vector3 direction, float projectileSpeed, float range, float damage)
	{
		this.direction = direction;
		this.projectileSpeed = projectileSpeed;
		this.range = range;
		this.damage = damage;
	}

	private void Start()
	{
		// Set projectile's gravity and velocity
		startPosition = transform.position;
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.gravityScale = 0f;
			rb.linearVelocity = direction * projectileSpeed;
		}

	}

	private void Update()
	{
		float distFromOrigin = (startPosition - transform.position).magnitude;

		if (distFromOrigin > range)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("bullet trigger entered");
		EnemyBase enemy = other.GetComponentInParent<EnemyBase>();
		Player player = other.GetComponentInParent<Player>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
			Destroy(gameObject);
		}
		else if (player != null)
		{
			player.TakeDamage(damage);
			Destroy(gameObject);
		}

	}

}
