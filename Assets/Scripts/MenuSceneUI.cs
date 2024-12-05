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
    [SerializeField] private Button _chooseSkinButton;

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
        _chooseSkinButton.onClick.AddListener(OnChooseSkinButtonClicked);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClicked);
        _chooseSkinButton.onClick.RemoveListener(OnChooseSkinButtonClicked);
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

    private void OnChooseSkinButtonClicked()
    {
        _sceneChanger.LoadByName("ChooseSkinScene");
    }
}