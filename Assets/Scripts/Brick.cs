using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private TextMeshPro _numberText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Gradient _colorGradient;

    private int _number;
    private Tween _textAnimation;

    public event Action<Brick> BrokeDown;

    private void Awake()
    {
        _textAnimation = _numberText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
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
        _spriteRenderer.color = _colorGradient.Evaluate(t);
    }

    private void PlayTextAnimation()
    {
        _textAnimation.Restart();
    }
}