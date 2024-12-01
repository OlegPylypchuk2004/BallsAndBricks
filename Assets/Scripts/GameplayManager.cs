using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private PausePanel _pausePanel;

    private bool _isCanLaunchBalls;
    private ObjectPool<Brick> _bricksPool;
    private List<Row> _rows;
    private List<IPickupable> _pickupables;
    private int _pickedBallsCount;
    private bool _isPaused;

    private void Start()
    {
        _rows = new List<Row>();
        _pickupables = new List<IPickupable>();

        Brick brickPrefab = Resources.Load<Brick>("Prefabs/Brick");
        _bricksPool = new ObjectPool<Brick>(brickPrefab, 10);

        Row rowPrefab = Resources.Load<Row>("Prefabs/Row");

        GameData gameData = GameDataManager.LoadGameData();
        RowData[] rowDatas = gameData.RowDatas.ToArray();

        if (rowDatas.Length > 0)
        {
            foreach (RowData rowData in rowDatas)
            {
                Row row = Instantiate(rowPrefab);
                row.transform.position = rowData.Position;

                _rows.Add(row);

                foreach (BrickData brickData in rowData.BrickDatas)
                {
                    Brick brick = Instantiate(brickPrefab);
                    brick.transform.SetParent(row.transform);
                    brick.transform.localPosition = new Vector2(brickData.Position.x, 0f);
                    brick.Number = brickData.Number;

                    row.AddBrick(brick);
                }
            }
        }
        else
        {
            Row row = Instantiate(rowPrefab);
            row.transform.position = new Vector2(0f, 3.5f);

            int bricksCount = Random.Range(3, row.Points.Length);

            for (int i = 0; i < bricksCount; i++)
            {
                Brick brick = Instantiate(brickPrefab);
                brick.transform.SetParent(row.transform);
                brick.transform.localPosition = new Vector2(row.Points[i].position.x, 0f);
                brick.Number = Random.Range(3, 25);
                brick.BrokeDown += OnBrickBrokeDown;

                row.AddBrick(brick);
            }

            _rows.Add(row);
        }

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

    private void OnBrickBrokeDown(Brick brick)
    {
        brick.BrokeDown -= OnBrickBrokeDown;

        ScoreManager.Instance.AddBrickDestroyCount();
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
                SaveGame();

                _isCanLaunchBalls = true;
                _ballLauncher.LaunchStarted += OnLaunchStarted;
            });
    }

    private void OnPickupableBallPicked(IPickupable pickupable)
    {
        pickupable.Picked -= OnPickupableBallPicked;
        _pickupables.Remove(pickupable);
        Destroy((pickupable as MonoBehaviour)?.gameObject);

        _pickedBallsCount++;
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