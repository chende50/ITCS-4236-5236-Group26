using UnityEngine;

public class EnemyBase : MonoBehaviour
{

	protected Player player;
	protected Transform playerTransform;
	protected const string PLAYER = "Player";

	[Header("Basic Attributes")]
	[SerializeField] protected int score;
	[SerializeField] protected float health;
	[SerializeField] protected float cost;

	[Header("Projectile Attributes")]
	[SerializeField] protected GameObject projectilePrefab;
	[SerializeField] protected float projectileSpeed = 5f;
	[SerializeField] protected float range = 5f;
	[SerializeField] protected float fireRate = 2f;
	[SerializeField] protected float damage = 1f;

	[Header("Movement Attributes")]
	[SerializeField] protected float moveSpeed = 2f;

	private int hitTracker = 0;

	protected virtual void Start()
	{
		player = GameObject.FindWithTag(PLAYER).GetComponent<Player>();

		if (player != null)
		{
			playerTransform = player.transform;
		}
	}

	protected virtual void Death()
	{
		int oldScore = score;
		score = score - hitTracker;
		Debug.Log(score);
		player.AddScore(score);
		Destroy(this.gameObject);
	}

	public void HitPlayer()
	{
		hitTracker++;
	}

	protected virtual void Update()
	{
		if (health <= 0)
		{
			Death();
		}
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;
	}

	protected virtual void ShootAtPlayer()
	{
		if ((playerTransform.position - transform.position).magnitude > range) { return; }
		if (projectilePrefab == null || playerTransform == null) return;

		Vector2 direction = (playerTransform.position - transform.position).normalized;

		GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

		BasicBullet projectileScript = projectile.AddComponent<BasicBullet>();
		projectileScript.Constructor(direction, projectileSpeed, range, damage, this);

	}

	protected void RotateEnemy()
	{
		Vector2 dirToTarget = GetDirToTarget();
		transform.up = dirToTarget;
	}

	protected void MoveEnemy()
	{
		Vector3 dirToTarget3 = GetDirToTargetVector3();

		transform.position += dirToTarget3 * moveSpeed * Time.deltaTime;
	}

	protected void runKinematicArrive()
	{
		RotateEnemy();
		MoveEnemy();
	}

	protected Vector2 GetDirToTarget()
	{
		Vector2 distToTarget = player.transform.position - transform.position;
		//Normalize vector to only use direction
		Vector2 dirToTarget = distToTarget.normalized;
		return dirToTarget;
	}

	protected Vector3 GetDirToTargetVector3()
	{
		Vector2 dirToTarget = GetDirToTarget();

		transform.up = dirToTarget;

		Vector3 dirToTarget3 = new Vector3(dirToTarget.x, dirToTarget.y, 0);
		return dirToTarget3;
	}

	public float GetCost()
	{
		return cost;
	}
}
