using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameInput gameInput;

    [Header("Player Statistics")]
    [SerializeField] private float playerSpeed;

    private void Start()
    {
        //Debug.Log("test");
    }

    private void Update()
    {
        //Vector2 mouseWorldPosition = gameInput.GetMouseWorldPosition();

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);

        transform.position += moveDir * Time.deltaTime * playerSpeed;
    }
}
