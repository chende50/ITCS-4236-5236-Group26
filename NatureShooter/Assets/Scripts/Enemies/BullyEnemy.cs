using UnityEngine;

public class BullyEnemy : EnemyBase
{
    //range of area around the bully that determines if the target has been reached
    [SerializeField] protected float radiusOfSatisfaction = 1f;
    [SerializeField] protected float attackStrength = 10f;

    protected override void Update()
    {
        base.Update();

        //Points from this bully to the player
        Vector3 toTarget = player.transform.position - transform.position;
        runKinematicArrive(toTarget);
        if(toTarget.magnitude <= range)
        {
            attack(attackStrength);
        }
    }

    private void runKinematicArrive(Vector3 toTarget)
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

    private void attack(float damage)
    {
        //TODO: Figure out player health
        //player.health -= damage;
    }
}
