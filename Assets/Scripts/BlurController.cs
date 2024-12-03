using DG.Tweening;
using Krivodeling.UI.Effects;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    [SerializeField] private UIBlur _blur;

    public Tween Appear()
    {
        gameObject.SetActive(true);

        return DOTween.To(() => _blur.Intensity, x => _blur.Intensity = x, 1f, 0.125f)
             .SetEase(Ease.OutQuad);
    }

    public Tween Disappear()
    {
        return DOTween.To(() => _blur.Intensity, x => _blur.Intensity = x, 0f, 0.125f)
            .SetEase(Ease.InQuad)
            .OnKill(() =>
            {
                gameObject.SetActive(false);
            });
    }

    public float Intensity
    {
        get => _blur.Intensity;
        set => _blur.Intensity = value;
    }
}