using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	[SerializeField] private List<EnemyBase> enemiesToSpawn = new List<EnemyBase>();
	[SerializeField] private List<EnemyBase> affordableEnemies = new List<EnemyBase>();

	[SerializeField] private float budget;
	private float baseBudget;
	private float budgetMultiplier;
	private float lastBudget;
	private float budgetScalar = .2f;
	private int minutesTillBudgetMultiplierIncrease = 2;
	private int currMinute;

	[SerializeField] private Vector2 randomizeSpawnTimerBetween;
	[SerializeField] private Vector2 enemySpawnCountRange;
	[SerializeField] private Vector2 spawnDistanceFromPlayerRange;

	public float currentTime = 0f;
	private float lastSpawnedAt;
	private float nextSpawnAt;
	private float waitTime;

	private Player player;
	private const string PLAYER = "Player";

	// plan:
	// the room has a budget. that budget is spent spawning certain enemies. the script needs to select enemies it can afford. then, the script needs to choose one enemy from that list and spawn it.
	// it would also be nice if the script could spawn enemies in groups, maybe certain enemies spawn only in groups of 3 or something? or maybe i can make the groups dynamic. yes i will make the groups dynamic. so:
	// 0. choose a random amount in a range of time to wait
	// 1. wait for that much time to pass (if time < nextSpawnAt) then select new time first and save last time spawned at
	// 2. populate list of affordable enemies (function for that)
	// 3. randomly select an enemy to spawn
	// 4. randomly select an amount of enemies to spawn (and make sure it doesn't exceed budget...) (choose amount of enemies, add up total cost, subtract 1 enemy until under cost?)
	// 5. spawn enemies...need some sort of spawnable area or something, or maybe just make it so they spawn within a certain range of the player for now
	// 6. loop 2 3 4 and 5 until budget is depleted
	// 7. add new amount to budget in some way based on time game has been running...maybe take the previous budget and add 1 for every x amount of seconds? and then maybe have a multiplier that increases every x minutes for linear and exponential scaling... linear in the beginning would be more impactful

	private void Start()
	{
		player = GameObject.FindWithTag(PLAYER).GetComponent<Player>();
		baseBudget = budget;
		waitTime = GetRandomWaitTime();
		nextSpawnAt = 0f;
		currMinute = 0;
		budgetMultiplier = 1;
		SpawnWave();
	}

	private void Update()
	{
		currentTime += Time.deltaTime;
		currMinute = Mathf.FloorToInt(currentTime / 60);
		budgetMultiplier = 1 + (Mathf.FloorToInt((float)currMinute / (float)minutesTillBudgetMultiplierIncrease) * budgetScalar);
		SpawnWave();
		//Debug.Log(currentTime);
	}

	public void SpawnWave()
	{
		if (currentTime < nextSpawnAt) return;
		Debug.Log("spawnwave activated");
		float timeSinceLastWave = currentTime - lastSpawnedAt;
		lastSpawnedAt = nextSpawnAt;
		waitTime = GetRandomWaitTime();
		nextSpawnAt = currentTime + waitTime;
		lastBudget = budget;

		FindAffordableEnemies();

		while (affordableEnemies.Count > 0)
		{
			Debug.Log("affordables enemies > 0");
			if (affordableEnemies.Count == 0) break;
			int randomEnemyIndex = Random.Range(0, affordableEnemies.Count);
			EnemyBase enemyToSpawn = affordableEnemies[randomEnemyIndex];
			int enemySpawnCount = Mathf.RoundToInt(Random.Range(enemySpawnCountRange.x, enemySpawnCountRange.y));
			if (enemySpawnCount == 0) { Debug.Log("Do not have 0 as the minimum"); enemySpawnCount = 1; }
			while (enemyToSpawn.GetCost() * enemySpawnCount > budget)
			{
				enemySpawnCount--;
			}
			float spawnDistanceFromPlayer = Random.Range(spawnDistanceFromPlayerRange.x, spawnDistanceFromPlayerRange.y);
			Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			Vector2 spawnPoint = (Vector2)player.transform.position + randomDirection * spawnDistanceFromPlayer;
			float spawnOffset = 1f;
			for (int i = 0; i < enemySpawnCount; i++)
			{
				if (i % 2 == 0)
				{
					spawnPoint = new Vector2(spawnPoint.x + spawnOffset * i, spawnPoint.y + spawnOffset * i / 2);
				}
				else
				{
					spawnPoint = new Vector2(spawnPoint.x - spawnOffset * i, spawnPoint.y + spawnOffset * i / 2);
				}
				EnemyBase enemy = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
				RemoveFromBudget(enemy.GetCost());
				Debug.Log(enemy);
			}
			FindAffordableEnemies();
		}
		float newBudget = (lastBudget + 4) * budgetMultiplier;
		SetBudget(newBudget);
		randomizeSpawnTimerBetween = new Vector2(randomizeSpawnTimerBetween.x + 2, randomizeSpawnTimerBetween.y + 2);
	}

	public void SpawnEnemy(EnemyBase enemy)
	{

	}

	public float GetRandomWaitTime()
	{
		return Random.Range(randomizeSpawnTimerBetween.x, randomizeSpawnTimerBetween.y);
	}

	public void FindAffordableEnemies()
	{
		affordableEnemies.Clear();
		foreach (EnemyBase enemy in enemiesToSpawn)
		{
			if (enemy.GetCost() <= budget)
			{
				affordableEnemies.Add(enemy);
			}
		}
	}

	public void AddToBudget(float amount)
	{
		budget += amount;
	}

	public void RemoveFromBudget(float amount)
	{
		budget -= amount;
	}

	public void SetBudget(float newBudget)
	{
		budget = newBudget;
	}

	public float GetBudget()
	{
		return budget;
	}

	public float GetNextSpawnAt()
	{
		return nextSpawnAt;
	}

}
