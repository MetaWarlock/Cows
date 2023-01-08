using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // The list of cows that the enemy can attack
    private List<GameObject> _cows;
    // The current target of the enemy (either a cow or the player)
    private GameObject _target;
    // The attack range of the enemy
    [SerializeField] float _attackRange = 2f;
    // The speed at which the enemy moves towards its target
    [SerializeField] float _movementSpeed = 5f;
    // The amount of time that the enemy should continue chasing the player after the player leaves the attack range
    [SerializeField] float _chaseDuration = 2f;
    // The attack speed of the enemy
    [SerializeField] float _attackSpeed = 1f;
    [SerializeField] float _aggroRange = 4f;
    // The time until the enemy's next attack
    private float _attackTimer;

    void Start()
    {
        // Find all of the cows in the scene and add them to the list
        _cows = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cow"));
        // Set the initial target to the closest cow
        _target = GetClosestTarget();
        // Set the initial attack timer to the attack speed
        _attackTimer = _attackSpeed;
    }

    void Update()
    {
        // If the enemy has a target
        if (_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);
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
                // Move towards the target
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime);

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the enemy collides with the player's aggro range collider
        if (other.tag == "Player")
        {
            // Set the target to the player
            _target = other.transform.gameObject;
            Debug.Log("Player entered aggro range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player leaves the attack range
        if (other.tag == "Player" && _target == other.gameObject)
        {
            Debug.Log("Player exits aggro range, start chasing!");
            // Start the chase timer
            StartCoroutine(ChaseTimer());           
        }
    }

    IEnumerator ChaseTimer()
    {
        // Wait for the chase duration
        yield return new WaitForSeconds(_chaseDuration);
        // If the target is still the player
        if (_target.tag == "Player")
        {
            Debug.Log("Stop Chasing, choose new target!");
            // Set the target back to the closest cow
            _target = GetClosestTarget();
        }
    }

    void Attack()
    {
        // If the target is a cow
        if (_target.tag == "Cow")
        {
            // Write something in the console
            Debug.Log("Attacking cow!");
            // Destroy the cow
            Destroy(_target);
            // Remove the cow from the list
            _cows.Remove(_target);
            // Set the target to the next closest cow
            _target = GetClosestTarget();
        }
        // If the target is the player
        else if (_target.tag == "Player")
        {
            // Write something in the console
            Debug.Log("Attacking player!");
        }
    }

    GameObject GetClosestTarget()
    {
        // Find the closest cow
        GameObject closestCow = GetClosestCow();
        // Find the player
        GameObject player = GameObject.FindWithTag("Player");

        // If there are no cows left, return the player
        if (closestCow == null) return player;
        // If the player doesn't exist, return the closest cow
        if (player == null) return closestCow;

        // Calculate the distance to the closest cow
        float cowDistance = Vector3.Distance(transform.position, closestCow.transform.position);
        // Calculate the distance to the player
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // If the player is closer than the closest cow
        if (playerDistance < _aggroRange)
        {
            Debug.Log("New target is a Player!");
            // Return the player
            return player;
            
        }
        // If the closest cow is closer than the player
        else
        {
            Debug.Log("New target is a Cow!");
            // Return the closest cow
            return closestCow;
        }
    }

    GameObject GetClosestCow()
    {
        // If there are no cows left, return null
        if (_cows.Count == 0) return null;

        // Set the first cow as the closest by default
        GameObject closestCow = _cows[0];
        // Set the distance to the closest cow to infinity
        float closestDistance = Mathf.Infinity;

        // For each cow in the list
        foreach (GameObject cow in _cows)
        {
            // Calculate the distance to the cow
            float distance = Vector3.Distance(transform.position, cow.transform.position);
            // If the distance to the cow is less than the distance to the closest cow
            if (distance < closestDistance)
            {
                // Set the cow as the closest cow
                closestCow = cow;
                // Set the distance to the closest cow to the distance to the cow
                closestDistance = distance;
            }
        }

        // Return the closest cow
        return closestCow;
    }
}
