using System.Collections;
using UnityEngine;

// TODO:
// separate rotation and movement
// add rotation to attack

public class BullyEnemy : EnemyBase
{
	//range of area around the bully that determines if the target has been reached
	[SerializeField] protected float radiusOfSatisfaction = 1f;
	[SerializeField] protected float attackDuration;
	[SerializeField] protected GameObject rightPunch;
	[SerializeField] protected GameObject leftPunch;

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
		currentPunch = rightPunch;
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

	private void IdlingBehavior()
	{

	}

	// Chasing -> Idling
	// ✅ Chasing -> Attacking
	private void ChasingBehavior()
	{
		//Points from this bully to the player
		Vector2 distToTarget = player.transform.position - transform.position;
		// Chasing -> Attacking
		if (distToTarget.magnitude <= range)
		{
			SetState(States.Attacking);
			return;
		}
		runKinematicArrive();

		
	}

	int punchCount = 0;
	GameObject currentPunch;
	private void PunchPlayer()
	{	
		if ((playerTransform.position - transform.position).magnitude > range) { return; }
		if (projectilePrefab == null || playerTransform == null) return;

		Vector2 direction = (playerTransform.position - transform.position).normalized;

		if (punchCount % 2 == 0)
		{
			currentPunch = rightPunch;
		} else
		{
			currentPunch = leftPunch;
		}

		// each punch has to alternate (punch count modulo)
		// each punch has to SetActive(false) after attackduration seconds (maybe just repurpose attack time temp entirely? i don't think it's necessary after this)
		// punch timer has to wait attackduration until starting the next attack

	}

	private IEnumerator AttackTimeTEMP()
	{
		PunchPlayer();
		currentPunch.SetActive(true);
		yield return new WaitForSeconds(attackDuration);
		currentPunch.SetActive(false);
	// 	yield return new WaitForSeconds(attackDuration / 2);
	// 	ShootAtPlayer();
	// 	yield return new WaitForSeconds(attackDuration / 2);
	// 	SetState(States.Chasing);
		punchCount++;
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
		RotateEnemy();
	}

	public float GetDamage()
	{
		return damage;
	}
}