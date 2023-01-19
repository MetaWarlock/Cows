using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{

    float speed = 0;
    // The damage that the sword will deal when it hits an enemy
    [SerializeField]
    private int _damage = 10;

    // The range of the sword's attack
    [SerializeField]
    private float _range = 1.5f;

    // A reference to the sword's attack animation
    [SerializeField]
    private AnimationClip attackAnimation;

    // The time it takes for the attack animation to complete
    private float attackAnimDuration;

    // A reference to the player's animator component
    private Animator animator;

    // A reference to the player's collider, which will be used to detect collisions with enemies
    private Collider playerCollider;

    // A flag to track whether the player is currently attacking
    public bool isAttacking = false;

    private bool _attackButton;

    private void Start()
    {
        // Get a reference to the player's animator component
        animator = GetComponent<Animator>();

        // Get a reference to the player's collider
        playerCollider = GetComponent<Collider>();

        // Get the duration of the attack animation
        attackAnimDuration = attackAnimation.length;

    }

    private void Update()
    {
        // Check if the player pressed the attack button
        if (_attackButton)
        {
            // Start the attack
            Attack();
        }
    }


    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }

    public void AttackInput(bool newAttackState)
    {
        _attackButton = newAttackState;
    }



    public void Attack()
    {
        if (!isAttacking) { 
        isAttacking = true;
        animator.SetTrigger("Attack");

        }

        // Play the attack animation




        // Wait for the attack animation to finish
        //yield return new WaitForSeconds(attackAnimDuration);

        //isAttacking = false;

        // Check for enemies in range
        //DetectEnemies();
    }

    private void StopAttack()
    {
        isAttacking = false;
        _attackButton = false;
    }

    private void DetectEnemies()
    {
        // Use Physics.OverlapSphere to detect enemies within range of the attack
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _range, LayerMask.GetMask("Enemies"));

        // Loop through the enemies and deal damage to them
        foreach (Collider enemy in enemiesInRange)
        {
            // Deal damage to the enemy
            //enemy.GetComponent<EnemyHealth>().TakeDamage(_damage);

            // Knock the enemy back
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            enemyRigidbody.AddExplosionForce(10.0f, transform.position, _range, 1.0f, ForceMode.Impulse);
        }
    }
}
