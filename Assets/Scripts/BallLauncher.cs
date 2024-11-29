using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetTransform;

    private List<Ball> _balls;
    private int _fallenBallsCount;
    private Vector2 _firstFallenBallPosition;

    public event Action LaunchStarted;
    public event Action BallsFallen;

    private void Start()
    {
        _balls = new List<Ball>();

        SpawnBall();
        _balls[0].transform.position = new Vector2(0f, -4.75f);
        _targetTransform.position = _balls[0].transform.position;
    }

    private void SpawnBall()
    {
        Ball ball = Instantiate(_ballPrefab);
        _balls.Add(ball);
    }

    public void TryLaunch()
    {
        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            _targetTransform.gameObject.SetActive(true);

            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(_balls[0].transform.position, direction);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _targetTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
            else
            {
                _targetTransform.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Launch(direction));

                LaunchStarted?.Invoke();
            }
        }
        else
        {
            _targetTransform.gameObject.SetActive(false);
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
        foreach (Ball ball in _balls)
        {
            ball.gameObject.SetActive(true);
            ball.Launch(direction);

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
            _targetTransform.position = _balls[0].transform.position;

            SpawnBall();

            for (int i = 0; i < _balls.Count; i++)
            {
                if (i != 0)
                {
                    _balls[i].gameObject.SetActive(false);
                }

                _balls[i].transform.position = _firstFallenBallPosition;
            }

            BallsFallen?.Invoke();
        }
    }
}