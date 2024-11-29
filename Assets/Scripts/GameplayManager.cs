using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private Transform[] _bricksPoints;

    private bool _isCanLaunchBalls;
    private ObjectPool<Brick> _bricksPool;
    private List<Brick> _bricks;

    private void Awake()
    {
        _bricks = new List<Brick>();

        Brick brickPrefab = Resources.Load<Brick>("Prefabs/Brick");
        _bricksPool = new ObjectPool<Brick>(brickPrefab, 10);
    }

    private void Start()
    {
        SpawnBricks();

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

    private void SpawnBricks()
    {
        MoveBricksAnimation().OnComplete(() =>
        {
            foreach (Transform brickPoint in _bricksPoints)
            {
                if (Random.Range(1, 3) >= 2)
                {
                    Brick brick = _bricksPool.GetObject();
                    brick.transform.position = brickPoint.transform.position;
                    brick.Destroyed += OnBrickDestroyed;

                    _bricks.Add(brick);
                }
            }
        });
    }

    private void OnBrickDestroyed(Brick brick)
    {
        brick.Destroyed -= OnBrickDestroyed;
        _bricks.Remove(brick);
        _bricksPool.ReturnObject(brick);
    }

    private Tween MoveBricksAnimation()
    {
        Sequence moveBricksSequence = DOTween.Sequence();

        foreach (Brick brick in _bricks)
        {
            Vector3 targetBrickPosition = brick.transform.position;
            targetBrickPosition.y -= 1;

            moveBricksSequence.Join
                (brick.transform.DOMove(targetBrickPosition, 0.25f)
                .SetEase(Ease.Linear));
        }

        return moveBricksSequence;
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

        ScoreManager.Instance.AddScore();
        SpawnBricks();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }
}