
using UnityEngine;
using UnityEngine.UI;

public class FlarePulse : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.15f;
    public float alphaAmount = 0.4f;

    private RectTransform rectTransform;
    private Graphic graphic;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        graphic = GetComponent<Graphic>();

        originalScale = rectTransform.localScale;

        if (graphic != null)
            originalColor = graphic.color;
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        float scale = 1f + pulse * scaleAmount;

        rectTransform.localScale = originalScale * scale;

        if (graphic != null)
        {
            Color color = originalColor;
            color.a = originalColor.a * (1f - pulse * alphaAmount);
            graphic.color = color;
        }
    }
}
