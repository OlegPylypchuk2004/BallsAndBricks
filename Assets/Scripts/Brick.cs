using System;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private TextMeshPro _numberText;

    private int _number;

    public event Action<Brick> Destroyed;

    private void Awake()
    {
        _number = Mathf.Clamp(ScoreManager.Instance.Score + UnityEngine.Random.Range(-5, 5), 1, int.MaxValue);
        UpdateView();
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
    }

    private void UpdateView()
    {
        _numberText.text = $"{_number}";
    }
}