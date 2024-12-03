using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshPro _scoreText;

    private Tween _textAnimation;

    private void Start()
    {
        ScoreManager.Instance.BricksDestroyCountChanged += OnBricksDestroyCountChanged;

        _textAnimation = _scoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.BricksDestroyCountChanged -= OnBricksDestroyCountChanged;
    }

    private void OnBricksDestroyCountChanged(int count)
    {
        _scoreText.text = $"{count}";
        _textAnimation.Restart();
    }
}