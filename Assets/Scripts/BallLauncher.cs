using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Target _target;
    [SerializeField] private TextMeshPro _ballsCountText;
    [SerializeField] private InputType _inputType;

    private ObjectPool<Ball> _ballsPool;
    private List<Ball> _balls;
    private int _fallenBallsCount;
    private Vector2 _firstFallenBallPosition;
    private Vector2 _tapPosition;
    private Vector2 _launchDirection;

    public event Action LaunchStarted;
    public event Action LaunchFinished;
    public event Action BallsFallen;

    private void Awake()
    {
        _balls = new List<Ball>();
    }

    private void Start()
    {
        Ball ballPrefab = Resources.Load<Ball>("Prefabs/Ball");
        _ballsPool = new ObjectPool<Ball>(ballPrefab, 10);
    }

    public void Initilize(float horizontalBallsPosition = 0f)
    {
        if (_balls.Count <= 0)
        {
            SpawnBall();
        }

        for (int i = 0; i < _balls.Count; i++)
        {
            _balls[i].transform.position = new Vector2(horizontalBallsPosition, -4.75f);
        }

        _firstFallenBallPosition = _balls[0].transform.position;

        _ballsCountText.text = $"x{_balls.Count}";
        _ballsCountText.transform.position = new Vector2(_balls[0].transform.position.x, _ballsCountText.transform.position.y);
    }

    public int BallsCount => _balls.Count;
    public float HorizontalBallsPosition => _balls[0].transform.position.x;

    public void SpawnBall(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Ball ball = _ballsPool.GetObject();
            _balls.Add(ball);
        }

        ResetBalls();
    }

    public void TryLaunch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _tapPosition = GetMouseWorldPosition();
        }

        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            _target.gameObject.SetActive(true);
            _target.transform.position = _firstFallenBallPosition;

            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(_balls[0].transform.position, direction);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _target.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
            else
            {
                _target.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Launch(direction));
            }
        }
        else
        {
            _target.gameObject.SetActive(false);
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = Vector2.zero;

        switch (_inputType)
        {
            case InputType.Classic:
                direction = GetMouseWorldPosition() - (Vector2)_balls[0].transform.position;
                break;

            case InputType.Inverted:
                direction = _tapPosition - GetMouseWorldPosition();
                break;

            default:
                direction = GetMouseWorldPosition() - (Vector2)_balls[0].transform.position;
                break;
        }

        direction = direction.normalized;

        return direction;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;

        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            return Vector2.zero;
        }

        return _camera.ScreenToWorldPoint(screenPos);
    }

    private IEnumerator Launch(Vector2 direction)
    {
        LaunchStarted?.Invoke();

        int notLaunchedBallsCount = _balls.Count;

        foreach (Ball ball in _balls)
        {
            ball.gameObject.SetActive(true);
            ball.Launch(direction);

            notLaunchedBallsCount--;

            if (notLaunchedBallsCount > 0)
            {
                _ballsCountText.text = $"x{notLaunchedBallsCount}";
            }
            else
            {
                _ballsCountText.text = "";
            }

            yield return new WaitForSeconds(0.075f);

            ball.Fallen += OnBallFallen;
        }

        LaunchFinished?.Invoke();
    }

    private void OnBallFallen(Ball ball, Vector2 position)
    {
        ball.Fallen -= OnBallFallen;

        _fallenBallsCount++;
        Sequence resetBallsSequence = DOTween.Sequence();

        if (_fallenBallsCount == 1)
        {
            _firstFallenBallPosition = position;
        }
        else
        {
            resetBallsSequence.Join
                (ball.transform.DOMove(_firstFallenBallPosition, 0.125f)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject));
        }

        if (_fallenBallsCount >= _balls.Count)
        {
            _fallenBallsCount = 0;

            resetBallsSequence.OnKill(() =>
            {
                _target.transform.position = _balls[0].transform.position;

                BallsFallen?.Invoke();
            });
        }
    }

    private void ResetBalls()
    {
        if (_balls.Count > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (i != 0)
                {
                    _balls[i].gameObject.SetActive(false);
                }

                _balls[i].transform.position = _firstFallenBallPosition;
            }

            _ballsCountText.text = $"x{_balls.Count}";
            _ballsCountText.transform.position = new Vector2(_balls[0].transform.position.x, _ballsCountText.transform.position.y);
        }
    }

    public void InstantReturnBalls()
    {
        Vector2 targetBallsPosition = new Vector2(0f, -4.755f);

        foreach (Ball ball in _balls)
        {
            ball.transform.position = targetBallsPosition;
        }
    }
}