using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        Time.fixedDeltaTime = 1f / 60f;

        SceneChanger.Instance.Load(1);
    }
}