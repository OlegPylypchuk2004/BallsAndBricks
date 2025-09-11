using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private bool _isLockHideAnimation;

    private Sequence _currentSequence;

    public static SceneChanger Instance { get; private set; }
    public bool IsLoading { get; private set; }
    public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    public event Action LoadStarted;
    public event Action LoadCompleted;
    public event Action Showed;
    public event Action Hided;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!_isLockHideAnimation)
        {
            Hide();
        }
    }

    public void Load(int index)
    {
        if (IsLoading)
        {
            return;
        }

        LoadAsync(index).Forget();
    }

    private async UniTaskVoid LoadAsync(int index)
    {
        await Show().AsyncWaitForCompletion();

        LoadStarted?.Invoke();

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        await SceneManager.LoadSceneAsync(index).ToUniTask();

        LoadCompleted?.Invoke();
    }

    private Sequence Show()
    {
        IsLoading = true;

        _eventSystem?.gameObject.SetActive(false);

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.SetLink(gameObject);

        _currentSequence.AppendCallback(() =>
        {
            _backgroundImage.gameObject.SetActive(true);
        });

        _currentSequence.Append(_backgroundImage.DOFade(1f, 0.25f)
            .From(0f)
            .SetEase(Ease.OutQuad));

        _currentSequence.AppendCallback(() =>
        {
            Showed?.Invoke();
        });

        return _currentSequence;
    }

    private Sequence Hide()
    {
        IsLoading = true;

        _eventSystem?.gameObject.SetActive(false);

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.SetLink(gameObject);

        _currentSequence.AppendCallback(() =>
        {
            _backgroundImage.gameObject.SetActive(true);
        });

        _currentSequence.Append(_backgroundImage.DOFade(0f, 0.25f)
            .SetEase(Ease.InQuad));

        _currentSequence.AppendCallback(() =>
        {
            _backgroundImage.gameObject.SetActive(false);
        });

        _currentSequence.AppendCallback(() =>
        {
            Hided?.Invoke();

            IsLoading = false;
        });

        _currentSequence.OnKill(() =>
        {
            _eventSystem?.gameObject.SetActive(true);
        });

        return _currentSequence;
    }
}