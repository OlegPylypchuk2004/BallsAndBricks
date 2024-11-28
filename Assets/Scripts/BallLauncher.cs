using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _camera;

    private void Update()
    {
        Vector2 direction = GetDirection();

        if (direction.y > 0)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(_ball.transform.position, direction);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _ball.Launch(direction);
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
}