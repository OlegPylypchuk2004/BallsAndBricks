using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private RectTransform _panelRectTransform;

    public void Appear()
    {
        gameObject.SetActive(true);
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
    }

    private void Disappear()
    {

    }
}