using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Frogger : MonoBehaviour
{
    private bool isInvulnerable = false;
    private bool isJumping = false;
    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    private SpriteRenderer spriteRenderer;
    private HeartManager heartManager;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    public Sprite hurtSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        heartManager = FindFirstObjectByType<HeartManager>();
    }

    void Update()
    {
        if (isJumping) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump(Vector3.up, Quaternion.Euler(0, 0, 0));
            return;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Jump(Vector3.left, Quaternion.Euler(0, 0, -270));
            return;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Jump(Vector3.down, Quaternion.Euler(0, 0, -180));
            return;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Jump(Vector3.right, Quaternion.Euler(0, 0, -90));
            return;
        }
    }
    
    private void Jump(Vector3 direction, Quaternion rotation) {

        isJumping = true;
        transform.rotation = rotation;
        Move(direction);
    }

    private void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Barrier")); // check if there is a collider at the destination
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Obstacle"));
        Collider2D water = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Water"));

        if (barrier != null) return; // if there is a collider, do not move


        if (platform != null) // if there is a platform, set the frog as a child of the platform so it moves with it
            transform.SetParent(platform.transform);
        else
            transform.SetParent(null);

        if (obstacle != null && platform == null && !isInvulnerable) // if there is an obstacle and no platform, the frog takes damage
        {
            transform.position = destination; // move the frog to the destination
            TakeDamage();
            return;
        }

        if (water != null && platform == null) // if the frog is in water and not on a platform, death
        {
            transform.position = destination;
            heartManager.setHealth(0);
            Death();
            return;
        }

        StartCoroutine(Leap(destination)); // start the leap coroutine to move the frog smoothly to the destination

    }

    private IEnumerator Leap(Vector3 destination) // Coroutine to move the frog smoothly between start and destination points
    {
        spriteRenderer.sprite = leapSprite; // change sprite to leap sprite when frog is moving
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        float duration = 0.125f; // 125 milliseconds

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsedTime += Time.deltaTime; // elapsed time is added
            yield return null;
        }

        transform.position = destination; // final position is set to the destination so where frog will end up
        spriteRenderer.sprite = idleSprite;
        isJumping = false;
        Debug.Log($" isJumping is: {isJumping}");
    }
    private void Death()
    {
        transform.rotation = Quaternion.identity; // reset rotation so frog is upright when it dies
        spriteRenderer.sprite = deadSprite;
        enabled = false; // disable the script so the frog cannot move anymore
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        int layer = other.gameObject.layer;

        // Skip conditions if
        if (!enabled) return;
        if (transform.parent != null) return; // ignore if riding a platform

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
        isJumping = false; // make sure frog can move again
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
    spriteRenderer.sprite = hurtSprite; // show hurt sprite
    float duration = 0.25f; // how long to show the hurt sprite
    yield return new WaitForSeconds(duration);

    // Only revert to idle if not dead and not leaping
    if (enabled && spriteRenderer.sprite != deadSprite)
        spriteRenderer.sprite = idleSprite;
}

}
