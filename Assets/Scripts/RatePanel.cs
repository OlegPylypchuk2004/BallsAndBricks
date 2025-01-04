using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatePanel : Panel
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rateButton;
    [SerializeField] private string _link;
    [SerializeField] private CanvasGroup _claimButtonCanvasGroup;
    [SerializeField] private TextMeshProUGUI _rewardAlreadyClaimedText;
    [SerializeField] private GameObject _coinsView;
    [SerializeField] private RectTransform _starsRectTransform;
    [SerializeField] private RectTransform[] _starRectTransforms;

    private Sequence _starsSequence;

    public override Sequence Appear()
    {
        PlayStarsAnimation();

        bool isRateRewardClaimed = PlayerDataManager.LoadPlayerData().IsRateRewardClaimed;

        if (isRateRewardClaimed)
        {
            _claimButton.gameObject.SetActive(false);
            _rewardAlreadyClaimedText.gameObject.SetActive(true);
            _rewardAlreadyClaimedText.text = "Reward already claimed";

            _coinsView.SetActive(false);
            _starsRectTransform.anchoredPosition = Vector3.zero;
        }
        else
        {
            _claimButton.gameObject.SetActive(true);
            _rewardAlreadyClaimedText.gameObject.SetActive(false);

            _claimButton.interactable = false;
            _claimButtonCanvasGroup.alpha = .5f;
        }

        return base.Appear();
    }

    public override Sequence Disappear()
    {
        DOTween.Kill(_starsSequence);

        return base.Disappear();
    }

    protected override void SubscribeOnEvents()
    {
        base.SubscribeOnEvents();

        _claimButton.onClick.AddListener(OnClaimButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
        _rateButton.onClick.AddListener(OnRateButtonClicked);
    }

    protected override void UnsubscribeOnEvents()
    {
        base.UnsubscribeOnEvents();

        _claimButton.onClick.RemoveListener(OnClaimButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _rateButton.onClick.RemoveListener(OnRateButtonClicked);
    }

    private void OnClaimButtonClicked()
    {
        _claimButton.interactable = false;

        PlayerData playerData = PlayerDataManager.LoadPlayerData();
        playerData.IsRateRewardClaimed = true;
        playerData.CoinsCount += 50;

        PlayerDataManager.SavePlayerData(playerData);

        _claimButton.gameObject.SetActive(false);
        _rewardAlreadyClaimedText.gameObject.SetActive(true);
        _rewardAlreadyClaimedText.text = "Reward claimed";
    }

    private void OnCloseButtonClicked()
    {
        Disappear();
    }

    private void OnRateButtonClicked()
    {
        Application.OpenURL(_link);

        _claimButton.interactable = true;
        _claimButtonCanvasGroup.alpha = 1f;
    }

    private void PlayStarsAnimation()
    {
        _starsSequence = DOTween.Sequence();

        _starsSequence.AppendInterval(3f);

        foreach (RectTransform starRectTransform in _starRectTransforms)
        {
            _starsSequence.Join(
                starRectTransform.DOPunchScale(Vector3.one * 0.25f, 0.25f)
                    .SetDelay(0.125f)
                );
        }

        _starsSequence.SetLink(gameObject);
        _starsSequence.SetLoops(-1);
    }
}