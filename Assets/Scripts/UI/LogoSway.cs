
using UnityEngine;

public class LogoSway : MonoBehaviour
{
    public float swaySpeed = 2f;
    public float swayAngle = 5f;

    private RectTransform rectTransform;
    private Quaternion originalRotation;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalRotation = rectTransform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAngle;

        rectTransform.localRotation =
            originalRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
