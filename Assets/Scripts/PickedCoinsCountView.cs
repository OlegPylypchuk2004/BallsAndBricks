using DG.Tweening;
using TMPro;
using UnityEngine;

public class PickedCoinsCountView : MonoBehaviour
{
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private RectTransform _coinsCountTextRectTransform;

    private Tween _textAnimation;
    private bool _isNeedAnimation;

    private void Awake()
    {
        _textAnimation = _coinsCountText.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f)
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
    }

    private void OnEnable()
    {
        _gameplayManager.PickedCoinsCountChanged += OnPickedCoinsCountChanged;
    }

    private void OnDisable()
    {
        _gameplayManager.PickedCoinsCountChanged -= OnPickedCoinsCountChanged;
    }

    private void OnPickedCoinsCountChanged(int coinsCount)
    {
        _coinsCountText.text = $"{coinsCount}";

        Vector2 preferredTextSize = _coinsCountText.GetPreferredValues();

        if (preferredTextSize.x > 200f)
        {
            preferredTextSize.x = 200f;
        }

        _coinsCountTextRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredTextSize.x);
        _coinsCountTextRectTransform.anchoredPosition = new Vector2(-250f + (200f - preferredTextSize.x) / 2, 0f);

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