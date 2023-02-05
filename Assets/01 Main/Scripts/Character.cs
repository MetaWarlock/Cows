using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _characterController;
    private MyPlayerInput _playerInput;
    private Animator _animator;

    private Vector3 _movementVelocity;
    private float _verticalVelocity;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity = -9.8f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<MyPlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0f,-45f,0f) * _movementVelocity;
        
        _animator.SetFloat("Speed",_movementVelocity.magnitude);
        
        _movementVelocity *= _moveSpeed * Time.deltaTime;

        if (_movementVelocity.sqrMagnitude > 0f )
        transform.rotation = Quaternion.LookRotation(_movementVelocity);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();

        if(_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f;
        }

        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

        _characterController.Move(_movementVelocity);
    }
}
