using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _enabledSoundButtonSprite;
    [SerializeField] private Sprite _disabledSoundButtonSprite;
    [SerializeField] private Button _rateButton;
    [SerializeField] private RatePanel _ratePanel;

    private void Start()
    {
        _bestScoreText.text = $"{PlayerDataManager.LoadPlayerData().BestScore}";

        if (!PlayerDataManager.LoadPlayerData().IsSoundDisabled)
        {
            _soundButtonImage.sprite = _enabledSoundButtonSprite;
        }
        else
        {
            _soundButtonImage.sprite = _disabledSoundButtonSprite;
        }
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(OnSoundButtonClicked);
        _rateButton.onClick.AddListener(OnRateButtonClicked);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
        _rateButton.onClick.RemoveListener(OnRateButtonClicked);
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

        SoundManager.Instance.PlayAudioClip(_buttonClickSound);
    }

    private void OnRateButtonClicked()
    {
        _ratePanel.Appear();
    }
}