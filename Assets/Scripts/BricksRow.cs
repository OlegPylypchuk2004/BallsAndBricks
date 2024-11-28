using System.Collections.Generic;
using UnityEngine;

public class BricksRow : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private Transform[] _bricksPoints;

    private List<Brick> _bricks;

    private void Awake()
    {
        _bricks = new List<Brick>();
        SpawnBricks();
    }

    private void SpawnBricks()
    {
        foreach (Transform brickPoint in _bricksPoints)
        {
            if (true)
            {
                Brick brick = Instantiate(_brickPrefab);
                brick.transform.SetParent(brickPoint.transform, false);

                _bricks.Add(brick);
            }
        }
    }
}