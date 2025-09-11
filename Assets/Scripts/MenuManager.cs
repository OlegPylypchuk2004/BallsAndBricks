using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        SceneChanger.Instance.Load(2);
    }
}