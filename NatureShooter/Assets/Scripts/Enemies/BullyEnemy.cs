using UnityEngine;
using System.Collections;

// TODO:
// separate rotation and movement
// add rotation to attack

public class BullyEnemy : EnemyBase
{
    //range of area around the bully that determines if the target has been reached
    [SerializeField] protected float radiusOfSatisfaction = 1f;
    [SerializeField] protected float attackStrength = 10f;
    [SerializeField] protected float attackDuration;

    private enum States
    {
        Idling,
        Chasing,
        Attacking
    }

    private States state;
    private States prevState;

    private void SetState(States newState)
    {
        prevState = state;
        state = newState;
        Debug.Log(state);
    }

    protected override void Start()
    {
        base.Start();
        SetState(States.Chasing);
    }

    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case (States.Idling):
                IdlingBehavior();
                break;
            case (States.Chasing):
                ChasingBehavior();
                break;
            case (States.Attacking):
                AttackingBehavior();
                break;
        }

    }

    private void runKinematicArrive(Vector2 distToTarget)
    {
        //Normalize vector to only use direction
        Vector2 dirToTarget = distToTarget.normalized;

        transform.up = dirToTarget;

        Vector3 dirToTarget3 = new Vector3(dirToTarget.x, dirToTarget.y, 0);

        //Move to target with speed
        transform.position += dirToTarget3 * moveSpeed * Time.deltaTime;
    }        

    private void IdlingBehavior()
    {
        
    }

    // Chasing -> Idling
    // ✅ Chasing -> Attacking
    private void ChasingBehavior()
    {
        //Points from this bully to the player
        Vector2 distToTarget = player.transform.position - transform.position;
        
        runKinematicArrive(distToTarget);

        // Chasing -> Attacking
        if(distToTarget.magnitude <= range)
        {
            SetState(States.Attacking);
        }
    }

    private IEnumerator AttackTimeTEMP()
    {
        yield return new WaitForSeconds(attackDuration / 2);
        ShootAtPlayer();
        yield return new WaitForSeconds(attackDuration / 2);
        SetState(States.Chasing);
        StopCoroutine(attackTimerCoroutine);
        attackTimerCoroutine = null;
    }

    private Coroutine attackTimerCoroutine;
    private void AttackingBehavior()
    {
        if (attackTimerCoroutine == null)
        {
            Debug.Log("attack timer coroutine started");
            attackTimerCoroutine = StartCoroutine(AttackTimeTEMP());
        }
       
    }
}