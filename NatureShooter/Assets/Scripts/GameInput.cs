using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    private InputActions inputActions;
    [SerializeField] Camera mainCam;
    public event EventHandler OnShootAction;

    private void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();
        inputActions.Player.Shoot.performed += Shoot_performed;
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector2 screenPos = inputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        return worldPos;
    }
    
}
