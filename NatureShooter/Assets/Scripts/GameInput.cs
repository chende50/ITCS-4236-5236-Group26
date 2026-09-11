using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    private InputActions inputActions;

    private void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetMouseWorldPosition()
    {
        Vector3 worldPositionVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 worldPosition = new Vector2(worldPositionVector3.x, worldPositionVector3.y);

        return worldPosition;
    }
    
}
