using DG.Tweening;
using UnityEngine;

public class PickupableBall : PickupableItem
{
    [SerializeField] private SpriteRenderer _outlineSpriteRenderer;
    [SerializeField] private Transform _circleTransform;
    [SerializeField] private PickupableBallPickedAnimation _pickedAnimationPrefab;
    [SerializeField] private AudioClip _pickupSound;

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
        _outlineSpriteRenderer.transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }

    public override void Pickup()
    {
        PickupableBallPickedAnimation pickedAnimation = Instantiate(_pickedAnimationPrefab);
        pickedAnimation.transform.position = transform.position;
        pickedAnimation.SetOutlineRotation(_outlineSpriteRenderer.transform.eulerAngles);

        SoundManager.Instance.PlayAudioClip(_pickupSound);

        base.Pickup();
    }
}