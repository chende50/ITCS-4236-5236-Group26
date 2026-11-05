using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{

	[Header("References")]
	[SerializeField] private GameInput gameInput;
	[SerializeField] private Transform gunHoldPosition;
	[SerializeField] private GameObject startWeapon;

	[Header("Player Statistics")]
	[SerializeField] private float playerSpeed;
	[SerializeField] public float playerHealth;

	private Camera mainCam;
	private GameObject currWeapon;

	private float score;
	private float targetScore;
	public float scoreMult = 1.0f;
	[SerializeField] float comboTimer = 3f;

	private void Start()
	{
		currWeapon = Instantiate(startWeapon, gunHoldPosition);
		mainCam = Camera.main;
		gameInput.OnShootAction += GameInput_OnShootAction;
	}

	private void GameInput_OnShootAction(object sender, System.EventArgs e)
	{
		Shoot();
	}

	private void Update()
	{
		//Vector2 mouseWorldPosition = gameInput.GetMouseWorldPosition();

		HandleMovement();
		HandleRotation();
	}

	private void HandleMovement()
	{
		Vector2 inputVector = gameInput.GetMovementVectorNormalized();

		Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);

		transform.position += moveDir * Time.deltaTime * playerSpeed;
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
}
