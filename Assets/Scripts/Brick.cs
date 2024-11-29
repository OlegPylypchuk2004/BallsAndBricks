using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private TextMeshPro _numberText;

    private int _number;
    private Tween _textAnimation;

    public event Action<Brick> Destroyed;

    private void Awake()
    {
        _number = Mathf.Clamp(ScoreManager.Instance.Score + UnityEngine.Random.Range(-5, 5), 1, int.MaxValue);

        _numberText.text = $"{_number}";

        _textAnimation = _numberText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
    }

    public void Hit()
    {
        _number--;

        if (_number <= 0)
        {
            Destroy(gameObject);
            Destroyed?.Invoke(this);
        }

        UpdateView();
        PlayTextAnimation();
    }

    private void UpdateView()
    {
        _numberText.text = $"{_number}";
    }

    private void PlayTextAnimation()
    {
        _textAnimation.Restart();
    }
}