using UnityEngine;
using UnityEngine.UI;

public class MenuPortraitAnimation : MonoBehaviour
{
    [Header("Character Frames")]
    public Sprite eyesOpen;
    public Sprite eyesHalfClosed;
    public Sprite eyesClosed;
    public Sprite sideEye;

    [Header("Blink Timing")]
    public float timeBetweenBlinks = 3f;
    public float halfClosedTime = 0.06f;
    public float closedTime = 0.1f;

    Image portrait;
    float blinkTimer = 0f;
    bool lookingAtSkull = false;

    void Awake()
    {
        portrait = GetComponent<Image>();
    }

    void OnEnable()
    {
        LookForward();
    }

    void Update()
    {
        if (lookingAtSkull) return;

        blinkTimer += Time.unscaledDeltaTime;

        float blinkTime = blinkTimer - timeBetweenBlinks;

        if (blinkTime < 0f)
        {
            portrait.sprite = eyesOpen;
        }
        else if (blinkTime < halfClosedTime)
        {
            portrait.sprite = eyesHalfClosed;
        }
        else if (blinkTime < halfClosedTime + closedTime)
        {
            portrait.sprite = eyesClosed;
        }
        else if (blinkTime < halfClosedTime * 2f + closedTime)
        {
            portrait.sprite = eyesHalfClosed;
        }
        else
        {
            blinkTimer = 0f;
            portrait.sprite = eyesOpen;
        }
    }

    public void LookAtSkull()
    {
        lookingAtSkull = true;
        blinkTimer = 0f;
        portrait.sprite = sideEye;
    }

    public void LookForward()
    {
        lookingAtSkull = false;
        blinkTimer = 0f;
        portrait.sprite = eyesOpen;
    }
}