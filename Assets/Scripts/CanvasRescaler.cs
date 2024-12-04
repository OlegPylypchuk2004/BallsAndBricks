using UnityEngine;
using UnityEngine.UI;

public class CanvasRescaler : MonoBehaviour
{
    [SerializeField] private CanvasScaler[] _canvasScalers;

    private void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (screenHeight / screenWidth <= 1.5f)
        {
            foreach (CanvasScaler canvasScaler in _canvasScalers)
            {
                canvasScaler.matchWidthOrHeight = 1;
            }
        }
        else
        {
            foreach (CanvasScaler canvasScaler in _canvasScalers)
            {
                canvasScaler.matchWidthOrHeight = 0;
            }
        }
    }
}