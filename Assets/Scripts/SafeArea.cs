using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    private void OnValidate()
    {
        _rectTransform ??= GetComponent<RectTransform>();
    }

    private void Start()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}