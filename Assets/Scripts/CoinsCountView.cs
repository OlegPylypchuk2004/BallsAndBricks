using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinsCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    private int _cashedCount;

    private void OnEnable()
    {
        _cashedCount = PlayerDataManager.LoadPlayerData().CoinsCount;
        _textMesh.text = $"{_cashedCount}";

        CoinsManager.CountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        CoinsManager.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        DOTween.To(() => _cashedCount, x => _cashedCount = x, count, 0.5f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                _textMesh.text = $"{_cashedCount}";
            });
    }
}