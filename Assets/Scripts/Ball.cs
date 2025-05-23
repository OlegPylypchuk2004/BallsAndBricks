using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool _isLaunched;

    public event Action<Ball, Vector2> Fallen;
    public event Action BrickHitted;

    private void FixedUpdate()
    {
        if (_isLaunched)
        {
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _speed;

            if (transform.position.y < -5f)
            {
                Fall();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickupableItem pickupable))
        {
            pickupable.Pickup();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Brick brick))
        {
            brick.Hit();

            BrickHitted?.Invoke();
        }
        else if (collision.gameObject.CompareTag("BottomMapBorder"))
        {
            Fall();
        }
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Launch(Vector2 direction)
    {
        _isLaunched = true;
        _rigidbody2D.AddForce(direction);
    }

    private void Fall()
    {
        _isLaunched = false;

        _rigidbody2D.velocity = Vector2.zero;

        Vector2 targetPosition = transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -3.35f, 3.35f);
        targetPosition.y = -4.75f;

        transform.position = targetPosition;

        Fallen?.Invoke(this, transform.position);
    }

    public void InstantFall()
    {
        Fall();
    }
}