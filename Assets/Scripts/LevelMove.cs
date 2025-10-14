using UnityEngine;
using System.Collections;

public class LevelMove : MonoBehaviour
{
    public enum MoveAxis { Vertical, Horizontal }

    [Header("Camera Movement")]
    public MoveAxis moveAxis = MoveAxis.Vertical; // choose in Inspector
    public Vector3 cameraMoveOffset = new Vector3(0, 12, 0);
    public float moveDuration = 1f;

    [Header("Trigger Settings")]
    public bool disableTriggerDuringMove = true;

    private bool isMoving = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isMoving)
            return;

        Vector3 playerPos = other.transform.position;
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow == null)
            return;

        Vector3 destination = Camera.main.transform.position;

        // --- Handle axis logic ---
        if (moveAxis == MoveAxis.Vertical)
        {
            if (playerPos.y < transform.position.y)
            {
                destination += cameraMoveOffset;
                print("Moving upward to next area");
            }
            else
            {
                destination -= cameraMoveOffset;
                print("Moving downward to previous area");
            }
        }
        else if (moveAxis == MoveAxis.Horizontal)
        {
            if (playerPos.x < transform.position.x)
            {
                destination += cameraMoveOffset;
                print("Moving right to next area");
            }
            else
            {
                destination -= cameraMoveOffset;
                print("Moving left to previous area");
            }
        }

        StartCoroutine(TransitionCamera(camFollow, destination));
    }

    private IEnumerator TransitionCamera(CameraFollow camFollow, Vector3 destination)
    {
        isMoving = true;

        yield return StartCoroutine(camFollow.MoveCameraTo(destination, moveDuration));

        print("Camera move complete");
        isMoving = false;
    }
}
