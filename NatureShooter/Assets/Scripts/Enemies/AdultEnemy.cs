using UnityEngine;

public class AdultEnemy : BullyEnemy
{
    protected enum States
    {
        Pursuit,
        Range,
        Melee
    }

    protected States currentState = States.Pursuit;

    protected override void Update()
    {
        if (fireCountdown <= 0f)
        {
            if (toTarget.magnitude < range)
            {
                currentState = States.Melee;
            }
            else
            {
                currentState = States.Range;
            }
        }

        if (fireCountdown > 0f)
        {
            // Decrease the countdown timer
            fireCountdown -= Time.deltaTime;
        }

        switch (currentState)
        {
            case States.Pursuit:
                toTarget = player.transform.position - transform.position;
                runKinematicArrive(toTarget);

                break;

            case States.Range:
                break;

            case States.Melee:
                break;
        }
    }

    protected override void attack(float damage)
    {
        
        switch (currentState)
        {
            case States.Range:
                break;

            case States.Melee:
                break;
        }
        
        fireCountdown = fireRate;

        return;
    }
}
