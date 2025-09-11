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
    [SerializeField] private Button _skinsButton;
    [SerializeField] private Image _rateButtonImage;
    [SerializeField] private Sprite _rateButtonDefaultSprite;
    [SerializeField] private Sprite _rateButtonNotificationSprite;

    private void Start()
    {
        PlayerData playerData = PlayerDataManager.LoadPlayerData();

        _bestScoreText.text = $"{playerData.BestScore}";

        if (!playerData.IsSoundDisabled)
        {
            _soundButtonImage.sprite = _enabledSoundButtonSprite;
        }
        else
        {
            _soundButtonImage.sprite = _disabledSoundButtonSprite;
        }

        if (!playerData.IsRateRewardClaimed)
        {
            _rateButtonImage.sprite = _rateButtonNotificationSprite;
        }
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(OnSoundButtonClicked);
        _rateButton.onClick.AddListener(OnRateButtonClicked);
        _skinsButton.onClick.AddListener(OnSkinsButtonClicked);

        _ratePanel.Disappeared += OnRatePanelDisappeared;
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
        _rateButton.onClick.RemoveListener(OnRateButtonClicked);
        _skinsButton.onClick.RemoveListener(OnSkinsButtonClicked);

        _ratePanel.Disappeared -= OnRatePanelDisappeared;
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

    private void OnSkinsButtonClicked()
    {
        SceneChanger.Instance.Load(3);
    }

    private void OnRatePanelDisappeared(Panel panel)
    {
        if (PlayerDataManager.LoadPlayerData().IsRateRewardClaimed)
        {
            _rateButtonImage.sprite = _rateButtonDefaultSprite;
        }
    }
}