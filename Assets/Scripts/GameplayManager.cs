using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BricksRow _rowPrefab;
    [SerializeField] private Transform _rowsParentTransform;

    private List<BricksRow> _rows;

    private void Awake()
    {
        _rows = new List<BricksRow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnRow();
        }
    }

    private void SpawnRow()
    {
        MoveRows();

        BricksRow row = Instantiate(_rowPrefab, new Vector3(0f, 4f, 0f), Quaternion.identity, _rowsParentTransform);
        _rows.Add(row);
    }

    private void MoveRows()
    {
        foreach (BricksRow row in _rows)
        {
            row.MoveDown();
        }
    }
}