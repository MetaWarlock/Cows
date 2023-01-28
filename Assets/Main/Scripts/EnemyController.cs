using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _aggroRange = 4f;
    [SerializeField] float _attackSpeed = 1f;
    [SerializeField] float _targetUpdateTimer = 0.1f;
    // The time until the enemy's next attack
    private float _attackTimer;
    private GameObject _target;
    private Animator _animator;


    void Start()
    {
        _animator= GetComponentInChildren<Animator>();
        // Set the initial target to the closest cow
        _target = GetClosestTarget();
        // Set the initial attack timer to the attack speed
        _attackTimer = _attackSpeed;
        // Start the CheckTargets coroutine
        StartCoroutine(CheckTargets());
    }

    void Update()
    {
        // If the enemy has a target
        if (_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);
            // Look towards the target
            transform.LookAt(_target.transform);
            // If the target is within attack range
            if (distance <= _attackRange)
            {
                // If the attack timer is finished
                if (_attackTimer <= 0f)
                {
                    // Attack the target
                    Attack();
                    // Reset the attack timer
                    _attackTimer = _attackSpeed;
                }
                // If the attack timer is not finished
                else
                {
                    // Decrement the attack timer
                    _attackTimer -= Time.deltaTime;
                }
            }
            // If the target is not within attack range
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime);
            }
        }
    }
    IEnumerator CheckTargets()
    {
        // Infinite loop
        while (true)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(_targetUpdateTimer);
            // Set the target to the closest target
            _target = GetClosestTarget();
            // If the enemy has a target
            
        }
    }


    void Attack()
    {
        Debug.Log("Attacking!");
        _animator.SetTrigger("Attack_t");
    }

    GameObject GetClosestTarget()
    {
        // Set the initial closest target to null
        GameObject closestTarget = null;
        // Set the initial minimum distance to a large number
        float minDistance = Mathf.Infinity;
        // Iterate through the Targets list
        foreach (GameObject target in Targets.list)
        {
            // Calculate the distance to the target
            float distance = Vector3.Distance(transform.position, target.transform.position);
            // If the target is the player and is within the attack range
            if (target.tag == "Player" && distance <= _aggroRange)
            {
                // Set the closest target to the player
                closestTarget = target;
                // Set the minimum distance to the current distance
                minDistance = distance;
                // Break out of the loop
                break;
            }
            // If the distance is smaller than the current minimum distance
            else if (distance < minDistance)
            {
                // Set the closest target to the current target
                closestTarget = target;
                // Set the minimum distance to the current distance
                minDistance = distance;
            }
        }
        // Return the closest target
        return closestTarget;
    }
    public void BiteAction()
    {
        // If the target is a cow
        if (_target.tag == "Cow")
        {
            Debug.Log("Bite cow!");
            Destroy(_target);
            // Remove the cow from the list
            Targets.RemoveTarget(_target);
            // Set the target to the next closest cow
            _target = GetClosestTarget();
        }
        // If the target is the player
        else if (_target.tag == "Player")
        {
            // Write something in the console
            Debug.Log("Bite player!");
            // Set the target to the next closest cow
            _target = GetClosestTarget();
        }
    }
}


