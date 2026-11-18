using UnityEngine;

public class PressAnyKeyPulse : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float zoomAmount = 0.08f;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        float zoom = 1f + Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        rectTransform.localScale = originalScale * zoom;
    }
}