using System;
using System.Collections;
using UnityEngine;

public class Frogger : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
        StartCoroutine(Leap(destination));
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
}
