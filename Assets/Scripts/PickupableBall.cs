using DG.Tweening;
using UnityEngine;

public class PickupableBall : PickupableItem
{
    [SerializeField] private SpriteRenderer _outlineSpriteRenderer;
    [SerializeField] private Transform _circleTransform;

    private void OnEnable()
    {
        _outlineSpriteRenderer.DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject);

        _circleTransform.DOScale(0.25f, 0.25f)
            .From(0f)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }
}