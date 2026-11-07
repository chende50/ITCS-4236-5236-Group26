using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

	[Header("References")]
	[SerializeField] private GameInput gameInput;
	[SerializeField] private Transform gunHoldPosition;
	[SerializeField] private GameObject startWeapon;
	[SerializeField] private HealthUI HealthUI;
	private Rigidbody2D rb;

	[Header("Player Statistics")]
	[SerializeField] private float acceleration;
	[SerializeField] private float targetSpeed;
	[SerializeField] public float playerHealth;

	private Camera mainCam;
	private GameObject currWeapon;

	private float score;
	private float targetScore;
	public float scoreMult = 1.0f;
	[SerializeField] float comboTimer = 3f;

	private float startTime;
	private float currentTime;
	private Vector2 inputVector;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		currWeapon = Instantiate(startWeapon, gunHoldPosition);
		mainCam = Camera.main;
		gameInput.OnShootAction += GameInput_OnShootAction;
		startTime = Time.unscaledTime;
		HealthUI.SetMaxHealth(100f);
	}

	private void GameInput_OnShootAction(object sender, System.EventArgs e)
	{
		Shoot();
		//currentTime = 0f;
	}

	private void Update()
	{
		//Vector2 mouseWorldPosition = gameInput.GetMouseWorldPosition();
		inputVector = gameInput.GetMovementVectorNormalized();
		UpdateTimer();

	}

	private void FixedUpdate()
	{
		HandleMovement();
		HandleRotation();
	}

	private void HandleMovement()
	{
		Vector2 targetVelocity = inputVector * targetSpeed;

		rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
	}

	private void HandleRotation()
	{
		Vector3 mousePosition = gameInput.GetMouseWorldPosition();
		Vector2 mouseDirectionNormalized = (mousePosition - transform.position).normalized;
		transform.up = mouseDirectionNormalized;
	}

	private void Shoot()
	{
		BasicGun basicGunScript = currWeapon.GetComponent<BasicGun>();
		if (basicGunScript == null) { Debug.Log("No weapon script"); return; }
		GameObject bulletPrefab = basicGunScript.GetWeaponSO().GetBulletPrefab();
		if (bulletPrefab == null) { Debug.Log("No bullet"); return; }
		WeaponSO weaponSO = basicGunScript.GetWeaponSO();

		Vector3 mousePosition = gameInput.GetMouseWorldPosition();
		Vector2 mouseDirectionNormalized = (mousePosition - transform.position).normalized;

		GameObject projectile = Instantiate(bulletPrefab, basicGunScript.GetBulletPoint().position, basicGunScript.GetBulletPoint().rotation);

		BasicBullet projectileScript = projectile.AddComponent<BasicBullet>();
		projectileScript.Constructor(mouseDirectionNormalized, weaponSO.GetProjectileSpeed(), weaponSO.GetRange(), weaponSO.GetDamage());

	}

	public void TakeDamage(float damage)
	{
		playerHealth -= damage;

		playerHealth = Mathf.Clamp(playerHealth, 0f, 100f);
		HealthUI.SetHealth(playerHealth);
	}

	private IEnumerator ComboRoutine(float duration)
	{
		yield return new WaitForSeconds(duration);
		scoreMult = 1.0f;
	}

	private Coroutine comboRoutine;
	public void AddScore(int amount)
	{
		if (comboRoutine != null) { StopCoroutine(comboRoutine); }
		float duration = 2f;
		scoreMult += .1f;
		targetScore += amount * scoreMult;
		if (scoreAnimationRoutine != null)
		{
			StopCoroutine(scoreAnimationRoutine);
		}
		comboRoutine = StartCoroutine(ComboRoutine(comboTimer));
		scoreAnimationRoutine = StartCoroutine(ScoreAnimationRoutine(amount, duration));
	}

	private Coroutine scoreAnimationRoutine;
	[SerializeField] private AnimationCurve scoreAnimationCurve;
	private IEnumerator ScoreAnimationRoutine(int amount, float animationTime)
	{
		float elapsed = 0f;
		float duration = animationTime;
		float startScore = score;
		float endScore = targetScore;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			float curveT = scoreAnimationCurve.Evaluate(t);
			score = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, curveT));
			yield return null;
		}
		score = endScore;
		scoreAnimationRoutine = null;
	}

	public float GetScore()
	{
		return score;
	}

	private void UpdateTimer()
	{
		currentTime = Time.timeSinceLevelLoad;
	}

	public float GetTime()
	{
		return currentTime;
	}
}
