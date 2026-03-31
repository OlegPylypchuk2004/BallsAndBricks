using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        double targetFrameRate = 60;

        foreach (Resolution screenResolution in Screen.resolutions)
        {
            if (screenResolution.refreshRateRatio.value > 60)
            {
                targetFrameRate = screenResolution.refreshRateRatio.value;
            }
        }

        Application.targetFrameRate = (int)targetFrameRate;

        Time.fixedDeltaTime = 1f / 60f;

        SceneChanger.Instance.Load(1);
    }
}