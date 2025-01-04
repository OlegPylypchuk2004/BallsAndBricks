using UnityEngine;
using UnityEngine.UI;

public class RatePanel : Panel
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rateButton;
    [SerializeField] private string _link;

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
        _closeButton.onClick.RemoveListener(OnRateButtonClicked);
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
    }
}