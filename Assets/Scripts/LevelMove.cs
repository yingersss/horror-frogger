using UnityEngine;
using System.Collections;

public class LevelMove : MonoBehaviour
{
    public Vector3 cameraMoveOffset = new Vector3(0, 6, 0);  // how far to pan
    public float moveDuration = 1f;
    public bool disableTriggerAfterUse = true; // so it doesn't re-trigger

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                Vector3 destination = Camera.main.transform.position + cameraMoveOffset;
                StartCoroutine(TransitionCamera(camFollow, destination));
            }

            if (disableTriggerAfterUse)
                hasTriggered = true;
        }
    }

    private IEnumerator TransitionCamera(CameraFollow camFollow, Vector3 destination)
    {
        yield return StartCoroutine(camFollow.MoveCameraTo(destination, moveDuration));
        print("Camera move complete — next area active!");
    }
}
