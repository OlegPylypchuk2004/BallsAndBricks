using DG.Tweening;
using UnityEngine;

public class PickupableBall : PickupableItem
{
    private void OnEnable()
    {
        transform.DORotate(new Vector3(0f, 0f, -360f), 3.75f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(gameObject);
    }
}