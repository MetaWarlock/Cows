using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;

    private Vector2 _moveDirection;

    public void SetMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();

        HorizontalInput = _moveDirection.x;
        VerticalInput = _moveDirection.y;
    }


    private void OnDisable()
    {
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
