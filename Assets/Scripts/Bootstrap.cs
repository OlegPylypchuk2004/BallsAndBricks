using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
        Time.fixedDeltaTime = 1f / 60f;

        SceneManager.LoadScene("MenuScene");
    }
}