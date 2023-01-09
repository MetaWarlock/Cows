using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
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
     private bool isAttacking = false;

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
        if (Input.GetButtonDown("Fire1"))
        {
            // Start the attack
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        // Play the attack animation
        animator.SetTrigger("Attack");

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackAnimDuration);

        isAttacking = false;

        // Check for enemies in range
        DetectEnemies();
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
