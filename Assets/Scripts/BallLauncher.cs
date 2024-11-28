using System;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _camera;

    public event Action LaunchStarted;
    public event Action BallsFallen;

    public void TryLaunch()
    {
        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(_ball.transform.position, direction * 12.5f);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _ball.Launch(direction);

                _ball.Fallen += OnBallFallen;

                LaunchStarted?.Invoke();
            }
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

        BallsFallen?.Invoke();
    }
}