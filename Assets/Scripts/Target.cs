using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _lineLength;
    [SerializeField] private float _reflectionLineLength;
    [SerializeField] private LayerMask _ignoreLayers;
    [SerializeField] private Transform _point;
    [SerializeField] private float _pointRadius;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _reflectionLineRenderer;

    private void Awake()
    {
        _lineRenderer.positionCount = 2;
        _reflectionLineRenderer.positionCount = 2;
    }

    private void LateUpdate()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, _lineLength, ~_ignoreLayers);

        if (hitInfo.collider != null)
        {
            Vector2 targetPointPosition = hitInfo.point;
            Vector2 offset = hitInfo.normal * _pointRadius;

            targetPointPosition += offset;

            _point.position = targetPointPosition;

            origin = hitInfo.point;
            direction = Vector2.Reflect(direction, hitInfo.normal);

            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, targetPointPosition);

            _reflectionLineRenderer.SetPosition(0, targetPointPosition);
            _reflectionLineRenderer.SetPosition(1, targetPointPosition + direction * _reflectionLineLength);
        }
        else
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, origin + direction * _lineLength);

            _reflectionLineRenderer.SetPosition(0, origin);
            _reflectionLineRenderer.SetPosition(1, origin + direction * _lineLength);
        }
    }
}