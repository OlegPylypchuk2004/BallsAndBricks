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

    //public void LoadData(BrickData brickData)
    //{
    //    transform.position = brickData.Position;
    //    _number = brickData.Number;

    //    UP();
    //}

    //public void RandomInit()
    //{
    //    _number = Mathf.Clamp(ScoreManager.Instance.BrickMovesCount + UnityEngine.Random.Range(0, 5), 1, int.MaxValue);

    //    UP();
    //}

    private void Update()
    {
        //UP();
    }

    private void UP()
    {
        _numberText.text = $"{_number}";

        _textAnimation = _numberText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();

        UpdateColor();
    }

    public int Number
    {
        get { return _number; }
        set { _number = value; UpdateColor(); }
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