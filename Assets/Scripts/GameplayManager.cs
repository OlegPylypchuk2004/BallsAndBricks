using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private Transform[] _bricksPoints;
    [SerializeField] private PausePanel _pausePanel;

    private bool _isCanLaunchBalls;
    private ObjectPool<Brick> _bricksPool;
    private List<Brick> _bricks;
    private List<IPickupable> _pickupables;
    private int _pickedBallsCount;
    private bool _isPaused;

    private void Awake()
    {
        _bricks = new List<Brick>();
        _pickupables = new List<IPickupable>();

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
        if (_isPaused)
        {
            return;
        }

        if (_isCanLaunchBalls)
        {
            _ballLauncher.TryLaunch();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SetPause(true);
        }
    }

    private void SpawnBricks()
    {
        MoveRowsAnimation().OnComplete(() =>
        {
            int randonPointsCount = UnityEngine.Random.Range(3, _bricksPoints.Length);
            List<Transform> randomBricksPoints = _bricksPoints.OrderBy(_ => UnityEngine.Random.value).Take(randonPointsCount).ToList();

            for (int i = 0; i < randomBricksPoints.Count; i++)
            {
                if (i == 0)
                {
                    PickupableBall pickupableBall = Instantiate(Resources.Load<PickupableBall>("Prefabs/PickupableBall"));
                    pickupableBall.transform.position = randomBricksPoints[i].position;
                    pickupableBall.Picked += OnPickupableBallPicked;

                    _pickupables.Add(pickupableBall);
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

        ScoreManager.Instance.AddBrickDestroyCount();
    }

    private Tween MoveRowsAnimation()
    {
        Sequence moveBricksSequence = DOTween.Sequence();

        foreach (Brick brick in _bricks)
        {
            Vector3 targetPosition = brick.transform.position;
            targetPosition.y -= 1;

            moveBricksSequence.Join
                (brick.transform.DOMove(targetPosition, 0.25f)
                .SetEase(Ease.Linear));
        }

        foreach (IPickupable pickupable in _pickupables)
        {
            Transform pickupableTransform = (pickupable as MonoBehaviour).transform;

            Vector3 targetPosition = pickupableTransform.position;
            targetPosition.y -= 1;

            moveBricksSequence.Join
                (pickupableTransform.DOMove(targetPosition, 0.25f)
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

        ScoreManager.Instance.AddBrickMove();
        SpawnBricks();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }

    private void OnPickupableBallPicked(IPickupable pickupable)
    {
        pickupable.Picked -= OnPickupableBallPicked;
        _pickupables.Remove(pickupable);
        Destroy((pickupable as MonoBehaviour)?.gameObject);

        _pickedBallsCount++;
    }

    private void SetPause(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;

            _pausePanel.Appear();
            _pausePanel.ContinueButtonClicked += OnPausePanelContinueButtonClicked;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnPausePanelContinueButtonClicked()
    {
        SetPause(false);
        _pausePanel.ContinueButtonClicked -= OnPausePanelContinueButtonClicked;
    }
}