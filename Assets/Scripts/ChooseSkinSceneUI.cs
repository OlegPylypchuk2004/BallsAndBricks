using UnityEngine;
using UnityEngine.UI;

public class ChooseSkinSceneUI : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        _sceneChanger.LoadByName("MenuScene");
    }
}