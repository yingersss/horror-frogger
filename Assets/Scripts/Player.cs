using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private HeartManager heartManager;
    private bool isInvulnerable = false;
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f;



    Vector2 movement;

    private void Awake()
    {
        heartManager = FindFirstObjectByType<HeartManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    void FixedUpdate()
    {
        // movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

        private void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;

        // Skip conditions if
        if (!enabled) return;

        bool isEnemyOrObstacle = 
            layer == LayerMask.NameToLayer("Enemy") || 
            layer == LayerMask.NameToLayer("Obstacle");

        if (isEnemyOrObstacle)
        {
            // Time-based cooldown check
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage();
                lastDamageTime = Time.time; // reset cooldown
            }
        }
    }

    private void TakeDamage()
    {
        heartManager.takeDamage();
        if (heartManager.health <= 0)
        {
            Death();
            return;
        }
        StartCoroutine(DamageFeedback());
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.1f); // Adjust duration as needed
        isInvulnerable = false;
    }

    private IEnumerator DamageFeedback()
    {
        //spriteRenderer.sprite = hurtSprite; // show hurt sprite
        float duration = 0.25f; // how long to show the hurt sprite
        yield return new WaitForSeconds(duration);

        // Only revert to idle if not dead and not leaping
        //if (enabled && spriteRenderer.sprite != deadSprite)
        //    spriteRenderer.sprite = idleSprite;
    }

    private void Death()
    {
        transform.rotation = Quaternion.identity; // reset rotation so frog is upright when it dies
        Debug.Log("Player has died.");
        // spriteRenderer.sprite = deadSprite;
        enabled = false; // disable the script so the frog cannot move anymore
    }


}
