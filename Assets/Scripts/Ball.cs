using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool _isLaunched;
    private Vector2 _direction;

    public event Action<Ball> Fallen;

    private void FixedUpdate()
    {
        if (_isLaunched)
        {
            _rigidbody2D.velocity = _direction * _speed;

            if (transform.position.y < -4f)
            {
                Fall();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rebound(collision);

        if (collision.gameObject.TryGetComponent(out Brick brick))
        {
            brick.Hit();
        }
    }

    public void Launch(Vector2 direction)
    {
        _isLaunched = true;
        _direction = direction;
    }

    private void Rebound(Collision2D collision)
    {
        if (!_isLaunched)
        {
            return;
        }

        Vector2 targetDirection = Vector2.zero;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            targetDirection += Vector2.Reflect(_direction, collision.contacts[i].normal);
        }

        targetDirection /= collision.contacts.Length;
        targetDirection = targetDirection.normalized;

        _direction = targetDirection;
    }

    private void Fall()
    {
        _isLaunched = false;

        _rigidbody2D.velocity = Vector2.zero;

        Vector2 targetPosition = transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -3.35f, 3.35f);
        targetPosition.y = -4;

        transform.position = targetPosition;

        Fallen?.Invoke(this);
    }
}