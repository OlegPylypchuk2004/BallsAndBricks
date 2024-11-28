using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BricksRow _rowPrefab;
    [SerializeField] private Transform _rowsParentTransform;
    [SerializeField] private BallLauncher _ballLauncher;

    private bool _isCanLaunchBalls;

    private List<BricksRow> _rows;

    private void Awake()
    {
        _rows = new List<BricksRow>();
    }

    private void Start()
    {
        SpawnRow();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }

    private void Update()
    {
        if (_isCanLaunchBalls)
        {
            _ballLauncher.TryLaunch();
        }
    }

    private void SpawnRow()
    {
        MoveRowsAnimation().OnComplete(() =>
        {
            BricksRow row = Instantiate(_rowPrefab, new Vector3(0f, 4f, 0f), Quaternion.identity, _rowsParentTransform);
            _rows.Add(row);
        });
    }

    private Tween MoveRowsAnimation()
    {
        Sequence moveRowsSequence = DOTween.Sequence();

        foreach (BricksRow row in _rows)
        {
            Vector3 targetRowPosition = row.transform.position;
            targetRowPosition.y -= 1;

            moveRowsSequence.Join
                (row.transform.DOMove(targetRowPosition, 0.25f)
                .SetEase(Ease.Linear));
        }

        return moveRowsSequence;
    }

    private void OnLaunchStarted()
    {
        _ballLauncher.LaunchStarted -= OnLaunchStarted;

        _isCanLaunchBalls = false;
        _ballLauncher.BallsFallen += OnBallsFallen;
    }

    private void OnBallsFallen()
    {
        _ballLauncher.BallsFallen -= OnBallsFallen;

        SpawnRow();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }
}