using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private Tween _textAnimation;
    private bool _isNeedAnimation;

    private void Awake()
    {
        _textAnimation = _scoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
    }

    private void Start()
    {
        ScoreManager.Instance.BricksDestroyCountChanged += OnBricksDestroyCountChanged;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.BricksDestroyCountChanged -= OnBricksDestroyCountChanged;
    }

    private void OnBricksDestroyCountChanged(int count)
    {
        _scoreText.text = $"{count}";

        if (_isNeedAnimation)
        {
            _textAnimation.Restart();
        }
        else
        {
            _isNeedAnimation = true;
        }
    }
}