using DG.Tweening;
using UnityEngine;

public class PickupableBallPickedAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _outlineSpriteRenderer;
    [SerializeField] private Transform _circleTransform;

    private void Start()
    {
        Sequence disappearSequence = DOTween.Sequence();

        disappearSequence.Append
            (_outlineSpriteRenderer.DOFade(0f, 0.5f)
            .From(1f)
            .SetEase(Ease.InQuad));

        disappearSequence.Join
            (_circleTransform.DOLocalMoveY(-0.75f, 0.5f)
            .SetEase(Ease.InBack));

        disappearSequence.Join
            (_circleTransform.DOScale(0f, 0.5f)
            .From(0.25f)
            .SetEase(Ease.InQuad));

        disappearSequence.SetLink(gameObject);
        disappearSequence.OnKill(() =>
        {
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        _outlineSpriteRenderer.transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }

    public void SetOutlineRotation(Vector3 rotation)
    {
        _outlineSpriteRenderer.transform.eulerAngles = rotation;
    }
}