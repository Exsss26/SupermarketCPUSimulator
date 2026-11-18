
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 30f;

    private RectTransform cloud;
    private RectTransform parentArea;

    void Start()
    {
        cloud = GetComponent<RectTransform>();
        parentArea = cloud.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 position = cloud.anchoredPosition;

        position.x += speed * Time.deltaTime;

        float scale = parentArea.lossyScale.x;

        float visibleWidth = parentArea.rect.width / scale;

        float rightLimit = visibleWidth / 2f + cloud.rect.width / 2f;
        float leftLimit = -visibleWidth / 2f - cloud.rect.width / 2f;

        if (position.x >= rightLimit)
        {
            position.x = leftLimit;
        }

        cloud.anchoredPosition = position;
    }
}
