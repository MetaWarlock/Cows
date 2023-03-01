using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;

    public bool MouseButtonDown;

    private Vector2 _moveDirection;

    public void SetMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();

        HorizontalInput = _moveDirection.x;
        VerticalInput = _moveDirection.y;
    }

    public void SetAttack()
    {
        if(Time.timeScale != 0)
        {
            MouseButtonDown = true;
        }
    }


    private void OnDisable()
    {
        MouseButtonDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
