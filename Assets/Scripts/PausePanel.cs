using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : Panel
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private ConfirmPanel _confirmPanel;

    protected override void SubscribeOnEvents()
    {
        base.SubscribeOnEvents();

        _continueButton.onClick.AddListener(OnContinueButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _menuButton.onClick.AddListener(OnMenuButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    protected override void UnsubscribeOnEvents()
    {
        base.UnsubscribeOnEvents();

        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.SetPause(false);
        });
    }

    private void OnRestartButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            if (ScoreManager.Instance.BrickMovesCount > 0)
            {
                _confirmPanel.Appear();
            }
            else
            {
                _gameplayManager.RestartGame();
            }
        });
    }

    private void OnMenuButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.GoToMenu();
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