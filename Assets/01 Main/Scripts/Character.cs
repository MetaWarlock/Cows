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

    //Enemy
    public bool IsPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _targetPlayer;

    //Health
    private Health _health;

    //Damage caster
    private DamageCaster _damageCaster;

    //Player slides
    private float _attackStartTime;
    [SerializeField] private float _attackSlideDuration;
    [SerializeField] private float _attackSlideSpeed;


    //State Machine
    public enum CharacterState
    {
        Normal,Attacking,Dead,BeingHit
    }

    public CharacterState CurrentState;

    //public bool Grounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _damageCaster = GetComponentInChildren<DamageCaster>();

        if(!IsPlayer )
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _targetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = _moveSpeed;
        }else
        {
            _playerInput = GetComponent<MyPlayerInput>();
        }
    }

    private void CalculatePlayerMovement()
    {

        if(_playerInput.MouseButtonDown && _characterController.isGrounded)
        {
            Debug.Log("ATAKA");
            SwitchStateTo(CharacterState.Attacking);
            return;
        }

        _movementVelocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0f,-45f,0f) * _movementVelocity;
        
        _animator.SetFloat("Speed",_movementVelocity.magnitude);
        
        _movementVelocity *= _moveSpeed * Time.deltaTime;

        if (_movementVelocity.sqrMagnitude > 0f )
        transform.rotation = Quaternion.LookRotation(_movementVelocity);
    }

    private void CalculateEnemyMovement()
    {
        if(Vector3.Distance(_targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(_targetPlayer.position);
            _animator.SetFloat("Speed",0.5f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0f);

            SwitchStateTo(CharacterState.Attacking);
        }
    }

    private void FixedUpdate()
    {

       // Grounded = _characterController.isGrounded;

        switch (CurrentState)
        {
            case CharacterState.Normal:
                {
                    if (IsPlayer)
                        CalculatePlayerMovement();
                    else
                        CalculateEnemyMovement();
                    break;
                }
            case CharacterState.Attacking:
                {
                    if (IsPlayer)
                    {
                        _movementVelocity = Vector3.zero;
                        if(Time.deltaTime < _attackStartTime + _attackSlideDuration)
                        {
                            float timePassed = Time.time - _attackStartTime;
                            float lerpTime = timePassed / _attackSlideDuration;
                            _movementVelocity = Vector3.Lerp(transform.forward * _attackSlideSpeed, Vector3.zero, lerpTime);
                        }
                    }
                    break;
                }
        }




        if(IsPlayer)
        {
            if (_characterController.isGrounded == false)
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

    private void SwitchStateTo(CharacterState newState)
    {
        //Clear Cache
        if(IsPlayer)
            _playerInput.MouseButtonDown = false;

        //Exiting state
        switch(CurrentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:

                if(_damageCaster != null)
                {
                    DisableDamageCaster();
                }

                break;
            case CharacterState.Dead:
                break;
            case CharacterState.BeingHit:
                break;
        }

        //Entering state
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:

                if (!IsPlayer)
                {
                    Quaternion newRotation = Quaternion.LookRotation(_targetPlayer.position - transform.position);
                    transform.rotation = newRotation;
                }

                _animator.SetTrigger("Attack");

                if (IsPlayer) _attackStartTime = Time.time;

                break;
        }

        CurrentState = newState;

       // Debug.Log(gameObject.name + " switched to " + CurrentState);
    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (_health!=null)
        {
            _health.ApplyDamage(damage);
        }

        if(!IsPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackerPos);
        }
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }
}
