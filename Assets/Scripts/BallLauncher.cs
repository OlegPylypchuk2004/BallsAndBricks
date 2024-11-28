using System;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetTransform;

    public event Action LaunchStarted;
    public event Action BallsFallen;

    private void Start()
    {
        _targetTransform.position = _ball.transform.position;
    }

    public void TryLaunch()
    {
        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            _targetTransform.gameObject.SetActive(true);

            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(_ball.transform.position, direction);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _targetTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }
            else
            {
                _targetTransform.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _ball.Launch(direction);

                _ball.Fallen += OnBallFallen;

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
        Vector2 direction = GetMouseWorldPosition() - (Vector2)_ball.transform.position;
        direction = direction.normalized;

        return direction;
    }

    private Vector2 GetMouseWorldPosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnBallFallen(Ball ball)
    {
        ball.Fallen -= OnBallFallen;

        _targetTransform.position = ball.transform.position;

        BallsFallen?.Invoke();
    }
}