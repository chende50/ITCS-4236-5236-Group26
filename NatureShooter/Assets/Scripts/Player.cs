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

    private void Start()
    {
        GameObject currWeapon = Instantiate(startWeapon, gunHoldPosition);
        mainCam = Camera.main;
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
        Vector3 mouseDirection = mousePosition - transform.position;
        Vector3 mouseDirectionNormalized = mouseDirection.normalized;
        transform.rotation = Quaternion.LookRotation(mouseDirectionNormalized, Vector3.up);
        transform.up = Vector3.up;
        Debug.Log(Quaternion.LookRotation(mouseDirectionNormalized));
    }
}
