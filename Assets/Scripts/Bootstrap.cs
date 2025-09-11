using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        Time.fixedDeltaTime = 1f / 60f;

        _sceneChanger.LoadByName("MenuScene");
    }
}