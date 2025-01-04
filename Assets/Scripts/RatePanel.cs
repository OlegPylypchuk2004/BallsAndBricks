using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RatePanel : Panel
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rateButton;
    [SerializeField] private string _link;
    [SerializeField] private CanvasGroup _claimButtonCanvasGroup;

    public override Sequence Appear()
    {
        _claimButton.interactable = false;
        _claimButtonCanvasGroup.alpha = .5f;

        return base.Appear();
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

    }

    private void OnCloseButtonClicked()
    {
        Disappear();
    }

    private void OnRateButtonClicked()
    {
        Application.OpenURL(_link);

        _rateButton.interactable = true;
        _claimButtonCanvasGroup.alpha = 1f;
    }
}