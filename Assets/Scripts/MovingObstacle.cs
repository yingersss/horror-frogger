using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Vector2 direction = Vector2.right; // movement direction
    public float speed = 1f;
    public int size = 1; // size of the obstacle for wrap-around logic

    private Vector3 leftEdge;
    private Vector3 rightEdge;

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
    
}
