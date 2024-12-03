using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _brickMovesCount;
    private int _brickDestroyCount;

    public event Action<int> BricksMoveCountChanged;
    public event Action<int> BricksDestroyCountChanged;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int brickMovesCount, int brickDestroyCount)
    {
        _brickMovesCount = brickMovesCount;
        BricksMoveCountChanged?.Invoke(_brickMovesCount);

        _brickDestroyCount = brickDestroyCount;
        BricksDestroyCountChanged?.Invoke(_brickDestroyCount);
    }

    public int BrickMovesCount
    {
        get { return _brickMovesCount; }
    }

    public int BrickDestroyCount
    {
        get { return _brickDestroyCount; }
    }

    public void AddBrickMove()
    {
        _brickMovesCount++;
        BricksMoveCountChanged?.Invoke(_brickMovesCount);
    }

    public void AddBrickDestroyCount()
    {
        _brickDestroyCount++;
        BricksDestroyCountChanged?.Invoke(_brickDestroyCount);
    }
}