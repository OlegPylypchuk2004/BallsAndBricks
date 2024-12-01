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

    public event Action<Brick> Destroyed;

    private void OnEnable()
    {
        _number = Mathf.Clamp(ScoreManager.Instance.BrickMovesCount + UnityEngine.Random.Range(0, 5), 1, int.MaxValue);

        _numberText.text = $"{_number}";

        _textAnimation = _numberText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();

        UpdateColor();
    }

    public void Hit()
    {
        _number--;

        if (_number <= 0)
        {
            Destroyed?.Invoke(this);
        }

        UpdateView();
        PlayTextAnimation();
        UpdateColor();
    }

    private void UpdateView()
    {
        _numberText.text = $"{_number}";
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