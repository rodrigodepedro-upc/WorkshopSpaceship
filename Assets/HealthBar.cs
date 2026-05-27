using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health health;          // the entity this bar tracks
    [SerializeField] RectTransform fillRect;  // the Tiled Image's RectTransform

    Vector2 fullSize;

    void Awake()
    {
        // Remember the full (100%) size so we can scale down from it.
        fullSize = fillRect.sizeDelta;
    }

    void OnEnable()
    {
        if (health != null)
            health.OnHealthChanged += UpdateBar;
    }

    void OnDisable()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(float fraction)
    {
        // Shrink the height; the Tiled image shows fewer segments instead of stretching.
        fillRect.sizeDelta = new Vector2(fullSize.x, fullSize.y * fraction);
    }
}
