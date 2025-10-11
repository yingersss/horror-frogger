using System;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;
    public float speed = 1f;
    public int size = 1;
    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero); // left edge of the screen
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right); // right edge of the screen
    }
    
    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x) // if moving right and the right edge of the object is beyond the right edge of the screen
        {
            // Moving right, wrap to left
            Vector3 position = transform.position; // get current position
            position.x = leftEdge.x - size; // set x position to the left edge of the screen minus the size of the object
            transform.position = position; // update the position
        }
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            // Moving left, wrap to right
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
