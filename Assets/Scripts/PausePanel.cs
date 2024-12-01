using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private RectTransform _panelRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;

    public event Action ContinueButtonClicked;

    public void Appear()
    {
        gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 0f);

        Sequence appearSequence = DOTween.Sequence();

        appearSequence.Append
            (_panelRectTransform.DOScale(1f, 0.25f)
            .From(0f)
            .SetEase(Ease.OutBack));

        appearSequence.Join
            (_fadeImage.DOFade(0.95f, 0.25f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        appearSequence.SetUpdate(true);
        appearSequence.SetLink(gameObject);

        appearSequence.OnComplete(() =>
        {
            _canvasGroup.interactable = true;
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        });
    }

    private void Disappear()
    {
        _canvasGroup.interactable = false;
        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);

        Sequence disappearSequence = DOTween.Sequence();

        disappearSequence.Append
            (_fadeImage.DOFade(0f, 0.25f)
            .SetEase(Ease.InQuad));

        disappearSequence.Join
            (_panelRectTransform.DOScale(0f, 0.25f)
            .SetEase(Ease.InQuad));

        disappearSequence.SetUpdate(true);
        disappearSequence.SetLink(gameObject);

        disappearSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);

            ContinueButtonClicked?.Invoke();
        });
    }

    private void OnContinueButtonClicked()
    {
        Disappear();
    }
}