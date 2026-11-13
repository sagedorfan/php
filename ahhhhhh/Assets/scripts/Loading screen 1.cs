using UnityEngine;
using UnityEngine.UI;

public class StarLoader : MonoBehaviour
{
    [System.Serializable]
    public class Star
    {
        public RectTransform rect;
        public Image image;
        public float delay;          // 0, 0.25, 0.5 like the CSS
    }

    public Star[] stars;

    [Header("Animation")]
    public float duration = 2f;
    public Vector2 startOffset = new Vector2(50f, 0f);
    public float riseDistance = 20f;
    public float spinDegrees = 360f;
    [Range(0f, 0.99f)] public float holdFraction = 0.1f; // the 0%-10% hold
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Look")]
    public Color starColor = new Color32(0xF8, 0xDE, 0x7E, 255); // #f8de7e

    Vector2[] basePos;

    void Awake()
    {
        basePos = new Vector2[stars.Length];
        for (int i = 0; i < stars.Length; i++)
        {
            basePos[i] = stars[i].rect.anchoredPosition;
            stars[i].image.color = new Color(starColor.r, starColor.g, starColor.b, 0f);
        }
    }

    void Update()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            Star s = stars[i];

            // Unscaled time so it keeps animating during loads / when timeScale = 0
            float t = Time.unscaledTime - s.delay;
            float p = t <= 0f ? 0f : Mathf.PingPong(t / duration, 1f); // "alternate"

            // 0-10% holds, 10-100% animates
            float k = Mathf.Clamp01((p - holdFraction) / (1f - holdFraction));
            float e = ease.Evaluate(k);

            // CSS y is down, Unity UI y is up; CSS rotate is clockwise, Unity's is counter-clockwise
            Vector2 offset = new Vector2(
                Mathf.Lerp(startOffset.x, 0f, e),
                Mathf.Lerp(0f, riseDistance, e));

            s.rect.anchoredPosition = basePos[i] + offset;
            s.rect.localRotation = Quaternion.Euler(0f, 0f, -spinDegrees * e);
            s.image.color = new Color(starColor.r, starColor.g, starColor.b, e);
        }
    }
}