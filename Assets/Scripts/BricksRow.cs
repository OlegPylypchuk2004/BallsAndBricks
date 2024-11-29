using System;
using System.Collections.Generic;
using UnityEngine;

public class BricksRow : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private Transform[] _bricksPoints;

    private List<Brick> _bricks;

    public event Action<BricksRow> Destroyed;

    private void Awake()
    {
        _bricks = new List<Brick>();
        SpawnBricks();
    }

    private void SpawnBricks()
    {
        foreach (Transform brickPoint in _bricksPoints)
        {
            if (UnityEngine.Random.Range(1, 3) >= 2)
            {
                Brick brick = Instantiate(_brickPrefab);
                brick.transform.SetParent(brickPoint.transform, false);

                _bricks.Add(brick);
                brick.Destroyed += OnBrickDestroyed;
            }
        }
    }

    private void OnBrickDestroyed(Brick brick)
    {
        _bricks.Remove(brick);

        if (_bricks.Count <= 0)
        {
            Destroy(gameObject);
            Destroyed?.Invoke(this);
        }
    }
}