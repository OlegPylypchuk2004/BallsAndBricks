using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
        Time.fixedDeltaTime = 1f / 60f;

        _sceneChanger.LoadByName("MenuScene");
    }
}