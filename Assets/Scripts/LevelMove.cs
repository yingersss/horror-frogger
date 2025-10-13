using UnityEngine;
using System.Collections;

public class LevelMove : MonoBehaviour
{
    public Vector3 cameraMoveOffset; // direction to move (e.g., (0, 6, 0))
    public float moveDuration = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                Vector3 destination = Camera.main.transform.position + cameraMoveOffset;
                StartCoroutine(camFollow.MoveCameraTo(destination, moveDuration));
            }
        }
    }
}
