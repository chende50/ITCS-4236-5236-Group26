using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    private InputActions inputActions;
    [SerializeField] Camera mainCam;

    private void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();
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
