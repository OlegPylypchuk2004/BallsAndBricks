using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _enabledSoundButtonSprite;
    [SerializeField] private Sprite _disabledSoundButtonSprite;

    private void Start()
    {
        _bestScoreText.text = $"{PlayerDataManager.LoadPlayerData().BestScore}";

        UpdateSoundButtonView(!PlayerDataManager.LoadPlayerData().IsSoundDisabled);
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(OnSoundButtonClicked);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
    }

    private void OnSoundButtonClicked()
    {
        PlayerData playerData = PlayerDataManager.LoadPlayerData();
        playerData.IsSoundDisabled = !playerData.IsSoundDisabled;

        PlayerDataManager.SavePlayerData(playerData);

        UpdateSoundButtonView(!playerData.IsSoundDisabled);
    }

    private void UpdateSoundButtonView(bool isEnabled)
    {
        if (isEnabled)
        {
            _soundButtonImage.sprite = _enabledSoundButtonSprite;
        }
        else
        {
            _soundButtonImage.sprite = _disabledSoundButtonSprite;
        }
    }
}