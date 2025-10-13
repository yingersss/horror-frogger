using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject frogger;
    public float speed;
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, frogger.transform.position);
        Vector2 direction = (frogger.transform.position - transform.position);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(distance < 4) // only move towards the player if they are within a certain distance
		{
            transform.position = Vector2.MoveTowards(this.transform.position, frogger.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
		}

    }
}
