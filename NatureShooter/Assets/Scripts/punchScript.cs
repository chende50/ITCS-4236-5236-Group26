using UnityEngine;

public class punchScript : MonoBehaviour
{
    private BullyEnemy bullyScript;
    private EnemyBase enemyBase;

    private void Start()
    {
        bullyScript = GetComponentInParent<BullyEnemy>();
        enemyBase = GetComponentInParent<EnemyBase>();
    }
    private void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("bullet trigger entered");
		Player player = other.GetComponentInParent<Player>();
		if (player != null)
		{
			player.TakeDamage(bullyScript.GetDamage());
			enemyBase.HitPlayer();
		}

	}
}
