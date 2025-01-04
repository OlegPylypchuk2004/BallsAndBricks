using UnityEngine;
using UnityEngine.UI;

public class RatePanel : Panel
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _closeButton;

    protected override void SubscribeOnEvents()
    {
        base.SubscribeOnEvents();

        _claimButton.onClick.AddListener(OnClaimButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    protected override void UnsubscribeOnEvents()
    {
        base.UnsubscribeOnEvents();

        _claimButton.onClick.RemoveListener(OnClaimButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnClaimButtonClicked()
    {

    }

    private void OnCloseButtonClicked()
    {
        Disappear();
    }
}