using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right; // movement direction
    public float speed = 1f;
    public int size = 1;

    private Vector3 leftEdge;
    private Vector3 rightEdge;

    private Rigidbody2D playerRb; // reference to the player's Rigidbody2D
    private bool playerOnPlatform = false; // is the player currently on the platform?

    private void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);   // left edge of the screen
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right); // right edge of the screen
    }

    private void Update()
    {
        // Wrap around logic
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x) 
        {
            Vector3 position = transform.position;
            position.x = leftEdge.x - size;
            transform.position = position;
        }
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 position = transform.position;
            position.x = rightEdge.x + size;
            transform.position = position;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (playerOnPlatform && playerRb != null) // if the player is on the platform, and we have a reference to their Rigidbody2D
        {
            playerRb.position += direction * speed * Time.deltaTime; // move the player along with the platform if player on platform
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // check if the colliding object is the player
        {
            playerRb = collision.attachedRigidbody; // get the player's Rigidbody2D
            playerOnPlatform = true; // set the flag to true
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // check if the exiting object is the player
        {
            playerOnPlatform = false; // set the flag to false
            playerRb = null; // clear the reference to the player's Rigidbody2D
        }
    }
}
