using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform gunHoldPosition;
    [SerializeField] private GameObject startWeapon;

    [Header("Player Statistics")]
    [SerializeField] private float playerSpeed;

    private Camera mainCam;
    private GameObject currWeapon;

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
        Vector3 initialPosition = projectile.transform.position;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null) { Debug.Log("bullet has no rigidbody"); return; }
        rb.gravityScale = 0;
        rb.linearVelocity = mouseDirectionNormalized * weaponSO.GetProjectileSpeed();

        float bulletLifeTime = weaponSO.GetRange() / weaponSO.GetProjectileSpeed();
        Destroy(projectile, bulletLifeTime);

    }
}
