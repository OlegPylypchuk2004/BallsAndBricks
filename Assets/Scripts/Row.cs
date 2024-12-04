using System;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private List<Brick> _bricks;
    private List<PickupableBall> _pickupableBalls;
    private List<PickupableCoin> _pickupableCoins;

    public event Action<Row> AllBricksBrokeDown;

    private void Awake()
    {
        _bricks = new List<Brick>();
        _pickupableBalls = new List<PickupableBall>();
        _pickupableCoins = new List<PickupableCoin>();
    }

    public Brick[] Bricks => _bricks.ToArray();
    public PickupableBall[] PickupableBalls => _pickupableBalls.ToArray();
    public PickupableCoin[] PickupableCoins => _pickupableCoins.ToArray();
    public Transform[] Points => _points;

    public void AddBrick(Brick brick)
    {
        _bricks.Add(brick);
        brick.BrokeDown += OnBrickBrokeDown;
    }

    public void AddPickupableBall(PickupableBall pickupableBall)
    {
        _pickupableBalls.Add(pickupableBall);
        pickupableBall.Picked += OnPickupableBallPicked;
    }

    public void AddPickupableCoin(PickupableCoin pickupableCoin)
    {
        _pickupableCoins.Add(pickupableCoin);
        pickupableCoin.Picked += OnPickupableCoinPicked;
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

    private void OnPickupableBallPicked(PickupableItem pickupableBall)
    {
        _pickupableBalls.Remove((PickupableBall)pickupableBall);
        pickupableBall.Picked -= OnPickupableBallPicked;
    }

    private void OnPickupableCoinPicked(PickupableItem pickupableCoin)
    {
        _pickupableCoins.Remove((PickupableCoin)pickupableCoin);
        pickupableCoin.Picked -= OnPickupableBallPicked;
    }
}