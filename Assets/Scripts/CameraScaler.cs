using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _minCameraSize;
    [SerializeField] private float _maxCameraSize;

    private void OnValidate()
    {
        if (_minCameraSize > _maxCameraSize)
        {
            _maxCameraSize = _minCameraSize;
        }
    }

    private void Awake()
    {
        float normalScreenCorrelation = 1920f / 1080f;
        float normalCameraSize = _camera.orthographicSize;
        float currentScreenCorrelation = (float)Screen.height / (float)Screen.width;
        float currentCameraSize = currentScreenCorrelation * normalCameraSize / normalScreenCorrelation;

        currentCameraSize = Mathf.Clamp(currentCameraSize, _minCameraSize, _maxCameraSize);

        _camera.orthographicSize = currentCameraSize;
    }
}