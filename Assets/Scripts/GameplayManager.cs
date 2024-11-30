using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private Transform[] _bricksPoints;

    private bool _isCanLaunchBalls;
    private ObjectPool<Brick> _bricksPool;
    private List<Brick> _bricks;
    private int _pickedBallsCount;

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

        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 1.5f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private void SpawnBricks()
    {
        MoveBricksAnimation().OnComplete(() =>
        {
            int randonPointsCount = Random.Range(3, _bricksPoints.Length);
            List<Transform> randomBricksPoints = _bricksPoints.OrderBy(_ => Random.value).Take(randonPointsCount).ToList();

            for (int i = 0; i < randomBricksPoints.Count; i++)
            {
                if (i == 0)
                {
                    PickupableBall pickupableBall = Instantiate(Resources.Load<PickupableBall>("Prefabs/PickupableBall"));
                    pickupableBall.transform.position = randomBricksPoints[i].position;
                    pickupableBall.Picked += OnPickupableBallPicked;
                }
                else
                {
                    Brick brick = _bricksPool.GetObject();
                    brick.transform.position = randomBricksPoints[i].position;
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

        _ballLauncher.SpawnBall(_pickedBallsCount);
        _pickedBallsCount = 0;

        ScoreManager.Instance.AddScore();
        SpawnBricks();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }

    private void OnPickupableBallPicked(IPickupable pickupable)
    {
        pickupable.Picked -= OnPickupableBallPicked;
        Destroy((pickupable as MonoBehaviour)?.gameObject);

        _pickedBallsCount++;
    }
}