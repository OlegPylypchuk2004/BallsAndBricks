using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool _isLaunched;
    private Vector2 _direction;

    private Vector2 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _isLaunched = false;
            transform.position = _startPos;
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_isLaunched)
        {
            _rigidbody2D.velocity = _direction * _speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rebound(collision);
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
}