using System;
using System.Collections;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball[] _balls;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetTransform;

    private int _fallenBallsCount;
    private Vector2 _firstFallenBallPosition;

    public event Action LaunchStarted;
    public event Action BallsFallen;

    private void Start()
    {
        _targetTransform.position = _balls[0].transform.position;
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
            ball.Launch(direction);

            yield return new WaitForSeconds(0.1f);

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

        if (_fallenBallsCount >= _balls.Length)
        {
            _fallenBallsCount = 0;
            _targetTransform.position = _balls[0].transform.position;

            for (int i = 0; i < _balls.Length; i++)
            {
                _balls[i].transform.position = _firstFallenBallPosition;
            }

            BallsFallen?.Invoke();
        }
    }
}