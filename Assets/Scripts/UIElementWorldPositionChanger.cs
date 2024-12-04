using UnityEngine;

public class UIElementWorldPositionChanger : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    private void OnValidate()
    {
        _rectTransform ??= GetComponent<RectTransform>();
    }

    private void Start()
    {
        float a = 1920f / 1080f;
        float b = (float)Screen.height / (float)Screen.width;

        Vector2 targetPosition = _rectTransform.anchoredPosition;
        targetPosition.y = b * targetPosition.y / a;

        _rectTransform.anchoredPosition = targetPosition;
    }
}