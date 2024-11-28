using DG.Tweening;
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
        MoveRowsAnimation().OnComplete(() =>
        {
            BricksRow row = Instantiate(_rowPrefab, new Vector3(0f, 4f, 0f), Quaternion.identity, _rowsParentTransform);
            _rows.Add(row);
        });
    }

    private Tween MoveRowsAnimation()
    {
        Sequence moveRowsSequence = DOTween.Sequence();

        foreach (BricksRow row in _rows)
        {
            Vector3 targetRowPosition = row.transform.position;
            targetRowPosition.y -= 1;

            moveRowsSequence.Join
                (row.transform.DOMove(targetRowPosition, 0.25f)
                .SetEase(Ease.Linear));
        }

        return moveRowsSequence;
    }
}