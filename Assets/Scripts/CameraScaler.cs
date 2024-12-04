using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        float normalScreenCorrelation = 1920f / 1080f;
        float normalCameraSize = _camera.orthographicSize;
        float currentScreenCorrelation = (float)Screen.height / (float)Screen.width;
        float currentCameraSize = currentScreenCorrelation * normalCameraSize / normalScreenCorrelation;

        currentCameraSize = Mathf.Clamp(currentCameraSize, normalCameraSize, 15f);

        _camera.orthographicSize = currentCameraSize;
    }
}