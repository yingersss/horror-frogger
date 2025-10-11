using System;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject heart;
    public int health, maxHealth;

    List<Heart> hearts = new List<Heart>();

    private void Start()
    {
        drawHearts();
    }
    public void drawHearts()
    {
        Debug.Log($"drawHearts called. maxHealth: {maxHealth}, health: {health}");
        clearHearts(); // clear hearts
        for (int i = 0; i < maxHealth; i++) // draw maxHealth number of hearts (3)
        {
            createHeart();
            Debug.Log($"Created heart {i}");
        }

        for (int i = 0; i < health; i++) // set each heart state and sprite to full
        {
            if (i < hearts.Count)
            {
                hearts[i].SetHeartState(HeartState.Full);
                Debug.Log($"Set heart {i} to full");
            }
            else
            {
                Debug.LogWarning($"Tried to set heart {i} to full, but only {hearts.Count} hearts exist.");
            }
        }
    }
    public void createHeart()
    {
        GameObject newHeart = Instantiate(heart);
        newHeart.transform.SetParent(transform, false); // parent to this gameobject

        Heart heartComponent = newHeart.GetComponent<Heart>();
        heartComponent.SetHeartState(HeartState.Full);
        hearts.Add(heartComponent);
    }

    public void setHealth(int health)
    {
        this.health = health;
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < health)
            {
                hearts[i].SetHeartState(HeartState.Full);
            }
            else
            {
                hearts[i].SetHeartState(HeartState.Empty);
            }
        }
	}

    public void clearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts.Clear(); // Clear the list to remove references to destroyed hearts
    }

    public void takeDamage()
    {
        int index = maxHealth - health;
        if (health > 0 && index >= 0 && index < hearts.Count)
        {
            hearts[index].SetHeartState(HeartState.Empty); // set the correct heart to empty
            health--; // decrease health by 1
            //Debug.Log("Took damage, health is now: " + health);
        }
        else if (health > 0)
        {
            //Debug.LogWarning($"HeartManager: Tried to access heart at invalid index {index}. Hearts count: {hearts.Count}");
            health--; // still decrease health to avoid infinite loop
        }
    }
}
