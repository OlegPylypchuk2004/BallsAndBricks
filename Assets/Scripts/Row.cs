using System;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private List<Brick> _bricks;

    public event Action<Row> AllBricksBrokeDown;

    private void Awake()
    {
        _bricks = new List<Brick>();
    }

    public Brick[] Bricks => _bricks.ToArray();
    public Transform[] Points => _points;

    public void AddBrick(Brick brick)
    {
        _bricks.Add(brick);
        brick.BrokeDown += OnBrickBrokeDown;
    }

    private void OnBrickBrokeDown(Brick brick)
    {
        _bricks.Remove(brick);
        brick.BrokeDown -= OnBrickBrokeDown;

        if (_bricks.Count <= 0)
        {
            AllBricksBrokeDown?.Invoke(this);
        }
    }
}