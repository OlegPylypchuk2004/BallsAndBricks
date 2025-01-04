using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private Sprite _enabledSoundButtonSprite;
    [SerializeField] private Sprite _disabledSoundButtonSprite;
    [SerializeField] private Button _rateButton;
    [SerializeField] private RatePanel _ratePanel;
    [SerializeField] private Button _skinsButton;

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
        _skinsButton.onClick.AddListener(OnSkinsButtonClicked);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
        _rateButton.onClick.RemoveListener(OnRateButtonClicked);
        _skinsButton.onClick.RemoveListener(OnSkinsButtonClicked);
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
        _sceneChanger.LoadByName("ChooseSkinScene");
    }
}