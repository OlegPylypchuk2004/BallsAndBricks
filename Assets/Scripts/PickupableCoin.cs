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
            .SetLink(gameObject);
    }

    public override void Pickup()
    {
        PickupableCoinPickedAnimation pickedAnimation = Instantiate(_pickedAnimationPrefab);
        pickedAnimation.transform.position = transform.position;

        base.Pickup();
    }
}