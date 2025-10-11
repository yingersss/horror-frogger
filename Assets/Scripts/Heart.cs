
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    private Image image;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Awake()
    {
    image = GetComponent<Image>();
    SetHeartState(HeartState.Full);
    }

    public void SetHeartState(HeartState state)
    {
        switch (state)
        {
            case HeartState.Empty:
                image.sprite = emptyHeart;
                Debug.Log($"SetHeartState: Set to Empty. Sprite: {(emptyHeart != null ? emptyHeart.name : "null")}");
                break;
            case HeartState.Full:
                image.sprite = fullHeart;
                Debug.Log($"SetHeartState: Set to Full. Sprite: {(fullHeart != null ? fullHeart.name : "null")}");
                break;
            default:
                Debug.LogError("Unknown heart state: " + state);
                break;
        }
    }

}

    public enum HeartState
    {
        Empty,
        Full
    }
