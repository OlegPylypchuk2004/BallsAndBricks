using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private BlurController _blurController;
    [SerializeField] private RectTransform _panelRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _closeButton;

    public Sequence Appear()
    {
        gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
        _blurController.Intensity = 0f;

        Sequence appearSequence = DOTween.Sequence();

        appearSequence.Append
            (_blurController.Appear());

        appearSequence.Join
            (_panelRectTransform.DOScale(1f, 0.125f)
            .From(0.95f)
            .SetEase(Ease.OutQuad));

        appearSequence.Join
            (_canvasGroup.DOFade(1f, 0.125f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        appearSequence.SetUpdate(true);
        appearSequence.SetLink(gameObject);

        appearSequence.OnKill(() =>
        {
            _canvasGroup.interactable = true;

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        });

        return appearSequence;
    }

    public Sequence Disappear()
    {
        _canvasGroup.interactable = false;

        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);

        Sequence disappearSequence = DOTween.Sequence();

        disappearSequence.Append
            (_blurController.Disappear());

        disappearSequence.Join
            (_panelRectTransform.DOScale(0.95f, 0.125f)
            .From(1f)
            .SetEase(Ease.InQuad));

        disappearSequence.Join
            (_canvasGroup.DOFade(0f, 0.125f)
            .From(1f)
            .SetEase(Ease.InQuad));

        disappearSequence.SetUpdate(true);
        disappearSequence.SetLink(gameObject);

        disappearSequence.OnKill(() =>
        {
            gameObject.SetActive(false);
        });

        return disappearSequence;
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

    }

    private void OnMenuButtonClicked()
    {

    }

    private void OnCloseButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.SetPause(false);
        });
    }
}