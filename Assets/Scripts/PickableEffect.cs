using DG.Tweening;
using TMPro;
using UnityEngine;

public class PickableEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _ballSpriteRenderer;
    [SerializeField] private SpriteRenderer _coinSpriteRenderer;
    [SerializeField] private TextMeshPro _textMesh;

    public void Initialize(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Ball:

                _coinSpriteRenderer.gameObject.SetActive(false);

                break;

            case EffectType.Coin:

                _ballSpriteRenderer.gameObject.SetActive(false);

                break;

            default:

                _coinSpriteRenderer.gameObject.SetActive(false);

                break;
        }

        PlayAnimation();
    }

    private Sequence PlayAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });

        sequence.AppendInterval(0.375f);

        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(true);
        });

        sequence.Join(
            transform.DOPunchScale(Vector3.one * 0.5f, 0.25f));

        sequence.Join(
            _ballSpriteRenderer.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.OutQuad));

        sequence.Join(
            _coinSpriteRenderer.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.OutQuad));

        sequence.Join(
            _textMesh.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.OutQuad));

        sequence.AppendInterval(1f);

        sequence.Join(
            _ballSpriteRenderer.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

        sequence.Join(
            _coinSpriteRenderer.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

        sequence.Join(
            _textMesh.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

        sequence.SetLink(gameObject);

        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });

        return sequence;
    }

    public enum EffectType
    {
        Ball,
        Coin
    }
}