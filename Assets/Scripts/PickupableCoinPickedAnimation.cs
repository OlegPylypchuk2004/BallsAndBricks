using DG.Tweening;
using UnityEngine;

public class PickupableCoinPickedAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        Sequence disappearSequence = DOTween.Sequence();

        disappearSequence.Append
            (_spriteRenderer.DOFade(0f, 0.25f)
            .From(1f)
            .SetEase(Ease.InQuad));

        disappearSequence.Join
            (_spriteRenderer.transform.DOScale(0f, 0.25f)
            .From(1f)
            .SetEase(Ease.InQuad));

        disappearSequence.SetLink(gameObject);
        disappearSequence.OnKill(() =>
        {
            Destroy(gameObject);
        });
    }
}