using UnityEngine;

public class FlowerEnemy : EnemyBase
{   


    private float fireCountdown = 0f;

    // Update is called once per frame
    protected override void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= range && fireCountdown <= 0f)
        {
            ShootAtPlayer();
            fireCountdown = fireRate;
        }

        if (fireCountdown > 0f)
        {
            // Decrease the countdown timer
            fireCountdown -= Time.deltaTime;
        }
    }

}
