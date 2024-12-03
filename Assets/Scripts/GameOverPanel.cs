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
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        int score = ScoreManager.Instance.BrickDestroyCount;
        int bestScore = 120;

        if (score >= bestScore)
        {
            _titleText.text = "New best score!";
        }
        else
        {
            _titleText.text = "Game over";
        }

        _scoreText.text = $"{score}";
        _bestScoreText.text = $"{bestScore}";
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
}