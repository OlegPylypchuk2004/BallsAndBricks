using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : Panel
{
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _closeButton;

    public override Sequence Appear()
    {
        int score = ScoreManager.Instance.BrickDestroyCount;

        PlayerData playerData = PlayerDataManager.LoadPlayerData();

        int bestScore = playerData.BestScore;

        if (score >= bestScore)
        {
            _titleText.text = "New best score!";
        }
        else
        {
            _titleText.text = "Game over";
        }

        return base.Appear().OnComplete(() =>
        {
            ShowBestScore(Mathf.Max(score, bestScore));
            ShowScore(score);
            ShowCoinsCount(playerData.CoinsCount);
        });
    }

    protected override void SubscribeOnEvents()
    {
        base.SubscribeOnEvents();

        _continueButton.onClick.AddListener(OnContinueButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    protected override void UnsubscribeOnEvents()
    {
        base.UnsubscribeOnEvents();

        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.RestartGame();
        });
    }

    private void OnCloseButtonClicked()
    {
        Disappear().OnComplete(() =>
        {
            _gameplayManager.RestartGame();
        });
    }

    private void ShowScore(int number)
    {
        int currentNumber = 0;

        DOTween.To(() => currentNumber, x => currentNumber = x, number, 0.5f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                _scoreText.text = $"{currentNumber}";
            });
    }
    private void ShowBestScore(int number)
    {
        int currentNumber = 0;

        DOTween.To(() => currentNumber, x => currentNumber = x, number, 0.5f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                _bestScoreText.text = $"{currentNumber}";
            });
    }

    private void ShowCoinsCount(int count)
    {
        int currentNumber = 0;

        DOTween.To(() => currentNumber, x => currentNumber = x, count, 0.5f)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                _coinsCountText.text = $"{currentNumber}";
            });
    }
}