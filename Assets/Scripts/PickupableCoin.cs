using DG.Tweening;
using UnityEngine;

public class PickupableCoin : PickupableItem
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PickupableCoinPickedAnimation _pickedAnimationPrefab;

    private void OnEnable()
    {
        _spriteRenderer.DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                _spriteRenderer.transform.DOScale(0.95f, 0.5f)
                .From(1f)
                .SetEase(Ease.InQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
            });
    }

    public override void Pickup()
    {
        PickupableCoinPickedAnimation pickedAnimation = Instantiate(_pickedAnimationPrefab);
        pickedAnimation.transform.position = transform.position;

        base.Pickup();
    }
}