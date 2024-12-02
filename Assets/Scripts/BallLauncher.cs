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
    [SerializeField] private TextMeshProUGUI _ballsCountText;

    private ObjectPool<Ball> _ballsPool;
    private List<Ball> _balls;
    private int _fallenBallsCount;
    private Vector2 _firstFallenBallPosition;

    public event Action LaunchStarted;
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

    public void Initilize()
    {
        if (_balls.Count <= 0)
        {
            SpawnBall();
        }

        for (int i = 0; i < _balls.Count; i++)
        {
            _balls[i].transform.position = new Vector2(0f, -4.75f);
        }

        _target.transform.position = _balls[0].transform.position;

        _ballsCountText.text = $"x{_balls.Count}";
    }

    public int BallsCount => _balls.Count;

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
        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            _target.gameObject.SetActive(true);

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

                LaunchStarted?.Invoke();
            }
        }
        else
        {
            _target.gameObject.SetActive(false);
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = GetMouseWorldPosition() - (Vector2)_balls[0].transform.position;
        direction = direction.normalized;

        return direction;
    }

    private Vector2 GetMouseWorldPosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator Launch(Vector2 direction)
    {
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
    }

    private void OnBallFallen(Ball ball, Vector2 position)
    {
        ball.Fallen -= OnBallFallen;

        _fallenBallsCount++;

        if (_balls[0] == ball)
        {
            _firstFallenBallPosition = position;
        }

        if (_fallenBallsCount >= _balls.Count)
        {
            _fallenBallsCount = 0;
            _target.transform.position = _balls[0].transform.position;

            BallsFallen?.Invoke();
        }
    }

    private void ResetBalls()
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
    }
}