using System;
using UnityEngine;

public class Frogger : MonoBehaviour
{
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
        transform.position += direction;
    }
    

}
