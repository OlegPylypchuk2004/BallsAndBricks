using DG.Tweening;
using System;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private BlurController _blurController;
    [SerializeField] private RectTransform _panelRectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private AudioClip _appearSound;
    [SerializeField] private AudioClip _disappearSound;
    [SerializeField] private bool _isPlaySound;

    public event Action<Panel> Appeared;
    public event Action<Panel> Disappeared;

    public virtual Sequence Appear()
    {
        gameObject.SetActive(true);
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
        _blurController.Intensity = 0f;

        if (_isPlaySound)
        {
            SoundManager.Instance.PlayAudioClip(_appearSound);
        }

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

            Appeared?.Invoke(this);
        });

        return appearSequence;
    }

    public virtual Sequence Disappear()
    {
        _canvasGroup.interactable = false;

        if (_isPlaySound)
        {
            SoundManager.Instance.PlayAudioClip(_disappearSound);
        }

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

            Disappeared?.Invoke(this);
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