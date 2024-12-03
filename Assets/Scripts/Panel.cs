using DG.Tweening;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private BlurController _blurController;
    [SerializeField] private RectTransform _panelRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

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

            SubscribeOnEvents();
        });

        return appearSequence;
    }

    public Sequence Disappear()
    {
        _canvasGroup.interactable = false;

        UnsubscribeOnEvents();

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

    protected virtual void SubscribeOnEvents()
    {

    }

    protected virtual void UnsubscribeOnEvents()
    {

    }
}