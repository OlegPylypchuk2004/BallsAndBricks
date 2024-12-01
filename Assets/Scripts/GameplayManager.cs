using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private PausePanel _pausePanel;

    private bool _isCanLaunchBalls;
    private List<Row> _rows;
    private int _pickedBallsCount;
    private bool _isPaused;

    private ObjectPool<Row> _rowsPool;
    private ObjectPool<Brick> _bricksPool;

    private IEnumerator Start()
    {
        _rows = new List<Row>();

        CreatePools();
        LoadGame();

        yield return new WaitForSeconds(0f);

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;
    }

    private void Update()
    {
        if (_isPaused || IsTouchOverUI())
        {
            return;
        }

        if (_isCanLaunchBalls)
        {
            _ballLauncher.TryLaunch();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SetPause(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 2f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GameDataManager.DeleteSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void CreatePools()
    {
        Row rowPrefab = Resources.Load<Row>("Prefabs/Row");
        _rowsPool = new ObjectPool<Row>(rowPrefab, 5);

        Brick brickPrefab = Resources.Load<Brick>("Prefabs/Brick");
        _bricksPool = new ObjectPool<Brick>(brickPrefab, 10);
    }

    private void SpawnRow()
    {
        Row row = _rowsPool.GetObject();
        row.transform.position = new Vector2(0f, 3.5f);

        int bricksCount = Random.Range(3, row.Points.Length);

        for (int i = 0; i < bricksCount; i++)
        {
            Brick brick = _bricksPool.GetObject();
            brick.transform.SetParent(row.transform);
            brick.transform.localPosition = new Vector2(row.Points[i].position.x, 0f);
            brick.Number = Random.Range(3, 25);
            brick.BrokeDown += OnBrickBrokeDown;

            row.AddBrick(brick);
        }

        _rows.Add(row);
        row.AllBricksBrokeDown += OnRowsAllBricksBrokeDown;
    }

    private void SaveGame()
    {
        GameData gameData = new GameData();

        foreach (Row row in _rows)
        {
            List<BrickData> brickDatas = new List<BrickData>();

            for (int i = 0; i < row.Bricks.Length; i++)
            {
                BrickData brickData = new BrickData(row.Bricks[i].Number, row.Bricks[i].transform.localPosition);
                brickDatas.Add(brickData);
            }

            RowData rowData = new RowData(row.transform.position, brickDatas.ToArray());
            gameData.SaveRow(rowData);
        }

        GameDataManager.SaveGameData(gameData);
    }

    private void LoadGame()
    {
        GameData gameData = GameDataManager.LoadGameData();
        RowData[] rowDatas = gameData.RowDatas.ToArray();

        if (rowDatas.Length > 0)
        {
            foreach (RowData rowData in rowDatas)
            {
                Row row = _rowsPool.GetObject();
                row.transform.position = rowData.Position;

                _rows.Add(row);

                foreach (BrickData brickData in rowData.BrickDatas)
                {
                    Brick brick = _bricksPool.GetObject();
                    brick.transform.SetParent(row.transform);
                    brick.transform.localPosition = new Vector2(brickData.Position.x, 0f);
                    brick.Number = brickData.Number;

                    row.AddBrick(brick);
                }

                row.AllBricksBrokeDown += OnRowsAllBricksBrokeDown;
            }
        }
        else
        {
            SpawnRow();
        }
    }

    private void OnBrickBrokeDown(Brick brick)
    {
        brick.BrokeDown -= OnBrickBrokeDown;

        brick.transform.SetParent(null);
        _bricksPool.ReturnObject(brick);
        ScoreManager.Instance.AddBrickDestroyCount();
    }

    private void OnRowsAllBricksBrokeDown(Row row)
    {
        _rowsPool.ReturnObject(row);
    }

    private Tween PlayMoveRowsAnimation()
    {
        Sequence moveRowsSequence = DOTween.Sequence();

        foreach (Row row in _rows)
        {
            Vector3 targetRowPosition = row.transform.position;
            targetRowPosition.y -= 1;

            moveRowsSequence.Join
                (row.transform.DOMove(targetRowPosition, 0.25f)
                .SetEase(Ease.Linear));
        }

        return moveRowsSequence;
    }

    private void OnLaunchStarted()
    {
        _ballLauncher.LaunchStarted -= OnLaunchStarted;

        _isCanLaunchBalls = false;
        _ballLauncher.BallsFallen += OnBallsFallen;
    }

    private void OnBallsFallen()
    {
        if (!_isPaused)
        {
            Time.timeScale = 1f;
        }

        _ballLauncher.BallsFallen -= OnBallsFallen;

        _ballLauncher.SpawnBall(_pickedBallsCount);
        _pickedBallsCount = 0;

        ScoreManager.Instance.AddBrickMove();

        PlayMoveRowsAnimation()
            .OnComplete(() =>
            {
                SpawnRow();
                SaveGame();

                _isCanLaunchBalls = true;
                _ballLauncher.LaunchStarted += OnLaunchStarted;
            });
    }

    private void SetPause(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;

            _pausePanel.Appear();
            _pausePanel.ContinueButtonClicked += OnPausePanelContinueButtonClicked;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void OnPausePanelContinueButtonClicked()
    {
        SetPause(false);
        _pausePanel.ContinueButtonClicked -= OnPausePanelContinueButtonClicked;
    }

    private bool IsTouchOverUI()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
        }

        return false;
    }
}