using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private TextMeshPro _numberText;
    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
    [SerializeField] private SpriteRenderer _gradientMaskSpriteRenderer;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private AudioClip _hitSound;

    private int _number;
    private Tween _textAnimation;
    private Sequence _appearSequence;

    public event Action<Brick> BrokeDown;

    private void Awake()
    {
        _textAnimation = _numberText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();

        _appearSequence = DOTween.Sequence();

        _appearSequence.Append
            (_backgroundSpriteRenderer.DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        _appearSequence.Join
            (_gradientMaskSpriteRenderer.DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        _appearSequence.Join
            (_numberText.DOFade(1f, 0.5f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        _appearSequence.SetLink(gameObject);
        _appearSequence.SetAutoKill(false);
    }

    private void OnEnable()
    {
        _appearSequence.Restart();
    }

    public int Number
    {
        get 
        { 
            return _number; 
        }
        set 
        { 
            _number = value;

            UpdateText();
            UpdateColor(); 
        }
    }

    public void Hit()
    {
        _number--;

        SoundManager.Instance.PlayAudioClip(_hitSound);

        if (_number <= 0)
        {
            BrokeDown?.Invoke(this);
        }

        UpdateText();
        PlayTextAnimation();
    }

    private void UpdateText()
    {
        _numberText.text = $"{_number}";

        UpdateColor();
    }

    private void UpdateColor()
    {
        float t = Mathf.Clamp01((float)_number / 50);

        Color targetBackgroundColor = _colorGradient.Evaluate(t);
        targetBackgroundColor.a = _backgroundSpriteRenderer.color.a;

        _backgroundSpriteRenderer.color = targetBackgroundColor;

        _gradientMaskSpriteRenderer.color = Color.Lerp(_backgroundSpriteRenderer.color, Color.black, 0.15f);
    }

    private void PlayTextAnimation()
    {
        _textAnimation.Restart();
    }
}