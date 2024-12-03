using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : Panel
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private PausePanel _pausePanel;

    protected override void SubscribeOnEvents()
    {
        base.SubscribeOnEvents();

        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    protected override void UnsubscribeOnEvents()
    {
        base.UnsubscribeOnEvents();

        _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnConfirmButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.RestartGame();
        });
    }

    private void OnCancelButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.SetPause(false);
        });
    }

    private void OnCloseButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.SetPause(false);
        });
    }
}