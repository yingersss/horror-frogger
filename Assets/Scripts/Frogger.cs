using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class Frogger : MonoBehaviour
{
    private bool isInvulnerable = false;

    private SpriteRenderer spriteRenderer;
    private HeartManager heartManager;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        heartManager = FindFirstObjectByType<HeartManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move(Vector3.up);
            Console.WriteLine("W key was pressed");
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, -270);
            Move(Vector3.left);
            Console.WriteLine("A key was pressed");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
            Move(Vector3.down);
            Console.WriteLine("S key was pressed");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            Move(Vector3.right);
            Console.WriteLine("D key was pressed");
        }
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

    }
    private void Death()
    {
        transform.rotation = Quaternion.identity; // reset rotation so frog is upright when it dies
        spriteRenderer.sprite = deadSprite;
        enabled = false; // disable the script so the frog cannot move anymore
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null && !isInvulnerable)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        heartManager.takeDamage();
        if (heartManager.health <= 0)
            Death();
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.1f); // Adjust duration as needed
        isInvulnerable = false;
    }
}
