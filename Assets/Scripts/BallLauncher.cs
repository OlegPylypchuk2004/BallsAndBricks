using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.DrawRay(_ball.transform.position, GetDirection());
        }

        if (Input.GetMouseButtonUp(0))
        {
            _ball.Launch(GetDirection());
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