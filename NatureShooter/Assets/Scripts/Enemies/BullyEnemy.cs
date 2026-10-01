using UnityEngine;

public class BullyEnemy : EnemyBase
{
    //range of area around the bully that determines if the target has been reached
    [Header("Movement Attributes")]
    [SerializeField] protected float radiusOfSatisfaction = 1f;

    [Header("Melee Attributes")]
    [SerializeField] protected float attackStrength = 10f;

    protected Vector3 toTarget;
    protected float fireCountdown = 0f;

    protected override void Update()
    {
        base.Update();

        //Points from this bully to the player
        toTarget = player.transform.position - transform.position;
        runKinematicArrive(toTarget);
        if(toTarget.magnitude <= range && fireCountdown > 0f)
        {
            attack(attackStrength);
        }
        if (fireCountdown > 0f)
        {
            // Decrease the countdown timer
            fireCountdown -= Time.deltaTime;
        }
    }

    protected void runKinematicArrive(Vector3 toTarget)
    {
        //See if player is within range of satisfaction
        if (toTarget.magnitude <= radiusOfSatisfaction)
        {
            //Stop moving if player is within radius of satisfaction
            return;
        }

        //Normalize vector to only use direction
        toTarget = toTarget.normalized;

        //Rotate smoothly to face the target
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

        //Move to target with speed
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    protected virtual void attack(float damage)
    {
        //TODO: Figure out player health
        //player.health -= damage;

        fireCountdown = fireRate;
    }
}
