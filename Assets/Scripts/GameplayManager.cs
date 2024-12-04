using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private BallLauncher _ballLauncher;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private Button _speedUpButton;

    private ObjectPool<Row> _rowsPool;
    private ObjectPool<Brick> _bricksPool;
    private ObjectPool<BrickDestroyParticles> _brickParticlesPool;
    private ObjectPool<PickupableItem> _pickupableBallPool;
    private ObjectPool<PickupableItem> _pickupableCoinPool;

    private List<Row> _rows;
    private bool _isCanLaunchBalls;
    private int _pickedBallsCount;
    private int _pickedCoinsCount;
    private bool _isPaused;
    private Coroutine _showSpeedUpButtonCoroutine;

    private void Start()
    {
        _rows = new List<Row>();

        CreatePools();
        LoadGame();

        _isCanLaunchBalls = true;
        _ballLauncher.LaunchStarted += OnLaunchStarted;

        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
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

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameDataManager.DeleteSave();
            _sceneChanger.LoadCurrent();
        }
#endif
    }

    private void CreatePools()
    {
        Row rowPrefab = Resources.Load<Row>("Prefabs/Row");
        _rowsPool = new ObjectPool<Row>(rowPrefab, 5);

        Brick brickPrefab = Resources.Load<Brick>("Prefabs/Brick");
        _bricksPool = new ObjectPool<Brick>(brickPrefab, 10);

        BrickDestroyParticles brickDestroyParticles = Resources.Load<BrickDestroyParticles>("Prefabs/BrickDestroyParticles");
        _brickParticlesPool = new ObjectPool<BrickDestroyParticles>(brickDestroyParticles, 10);

        PickupableBall pickupableBall = Resources.Load<PickupableBall>("Prefabs/PickupableBall");
        _pickupableBallPool = new ObjectPool<PickupableItem>(pickupableBall, 5);

        PickupableCoin pickupableCoin = Resources.Load<PickupableCoin>("Prefabs/PickupableCoin");
        _pickupableCoinPool = new ObjectPool<PickupableItem>(pickupableCoin, 5);
    }

    private void SpawnRow()
    {
        Row row = _rowsPool.GetObject();
        row.transform.position = new Vector2(0f, 3.5f);

        int pointsCount = Random.Range(5, row.Points.Length);
        List<Transform> randomBricksPoints = row.Points
            .OrderBy(_ => Random.value)
            .Take(pointsCount)
            .ToList();

        bool isSpawnCoin = Random.Range(0, 10) >= 5;

        for (int i = 0; i < randomBricksPoints.Count; i++)
        {
            Transform point = randomBricksPoints[i];

            if (i == 0)
            {
                PickupableItem pickupableBall = _pickupableBallPool.GetObject();
                pickupableBall.transform.SetParent(row.transform);
                pickupableBall.transform.position = point.position;
                pickupableBall.Picked += OnPickupableBallPicked;

                row.AddPickupableBall((PickupableBall)pickupableBall);
            }
            else
            {
                if (isSpawnCoin)
                {
                    isSpawnCoin = false;

                    PickupableItem pickupableCoin = _pickupableCoinPool.GetObject();
                    pickupableCoin.transform.SetParent(row.transform);
                    pickupableCoin.transform.position = point.position;
                    pickupableCoin.Picked += OnPickupableCoinPicked;

                    row.AddPickupableCoin((PickupableCoin)pickupableCoin);
                }
                else
                {
                    Brick brick = _bricksPool.GetObject();
                    brick.transform.SetParent(row.transform);
                    brick.transform.position = point.position;
                    brick.Number = Mathf.Clamp(ScoreManager.Instance.BrickMovesCount + Random.Range(0, 5), 1, int.MaxValue);
                    brick.BrokeDown += OnBrickBrokeDown;

                    row.AddBrick(brick);
                }
            }
        }

        _rows.Add(row);
        row.AllBricksBrokeDown += OnRowsAllBricksBrokeDown;
    }

    private void StartNewGame()
    {
        SpawnRow();

        _ballLauncher.Initilize();
    }

    private void SaveGame()
    {
        GameData gameData = new GameData();

        gameData.BrickMovesCount = ScoreManager.Instance.BrickMovesCount;
        gameData.BrickDestroyCount = ScoreManager.Instance.BrickDestroyCount;
        gameData.BallsCount = _ballLauncher.BallsCount;
        gameData.PickedCoinsCount = _pickedCoinsCount;
        gameData.HorizontalBallsPosition = _ballLauncher.HorizontalBallsPosition;

        foreach (Row row in _rows)
        {
            List<BrickData> brickDatas = new List<BrickData>();

            for (int i = 0; i < row.Bricks.Length; i++)
            {
                BrickData brickData = new BrickData(row.Bricks[i].Number, row.Bricks[i].transform.localPosition);
                brickDatas.Add(brickData);
            }

            List<PickupableBallData> pickupableBallDatas = new List<PickupableBallData>();

            for (int i = 0; i < row.PickupableBalls.Length; i++)
            {
                PickupableBallData pickupableBallData = new PickupableBallData(row.PickupableBalls[i].transform.position);
                pickupableBallDatas.Add(pickupableBallData);
            }

            List<PickupableCoinData> pickupableCoinDatas = new List<PickupableCoinData>();

            for (int i = 0; i < row.PickupableCoins.Length; i++)
            {
                PickupableCoinData pickupableCoinData = new PickupableCoinData(row.PickupableCoins[i].transform.position);
                pickupableCoinDatas.Add(pickupableCoinData);
            }

            RowData rowData = new RowData(row.transform.position, brickDatas.ToArray(), pickupableBallDatas.ToArray(), pickupableCoinDatas.ToArray());
            gameData.SaveRow(rowData);
        }

        GameDataManager.SaveGameData(gameData);
    }

    private void LoadGame()
    {
        GameData gameData = GameDataManager.LoadGameData();

        ScoreManager.Instance.Initialize(gameData.BrickMovesCount, gameData.BrickDestroyCount);

        _pickedCoinsCount = gameData.PickedCoinsCount;

        _ballLauncher.SpawnBall(gameData.BallsCount);
        _ballLauncher.Initilize(gameData.HorizontalBallsPosition);

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
                    brick.BrokeDown += OnBrickBrokeDown;

                    row.AddBrick(brick);
                }

                foreach (PickupableBallData pickupableBallData in rowData.PickupableBallDatas)
                {
                    PickupableBall pickupableBall = (PickupableBall)_pickupableBallPool.GetObject();
                    pickupableBall.transform.SetParent(row.transform);
                    pickupableBall.transform.localPosition = new Vector2(pickupableBallData.Position.x, 0f);
                    pickupableBall.Picked += OnPickupableBallPicked;

                    row.AddPickupableBall(pickupableBall);
                }

                foreach (PickupableCoinData pickupableCoinData in rowData.PickupableCoinDatas)
                {
                    PickupableCoin pickupableCoin = (PickupableCoin)_pickupableCoinPool.GetObject();
                    pickupableCoin.transform.SetParent(row.transform);
                    pickupableCoin.transform.localPosition = new Vector2(pickupableCoinData.Position.x, 0f);
                    pickupableCoin.Picked += OnPickupableCoinPicked;

                    row.AddPickupableCoin(pickupableCoin);
                }

                row.AllBricksBrokeDown += OnRowsAllBricksBrokeDown;
            }
        }
        else
        {
            StartNewGame();
        }
    }

    public void RestartGame()
    {
        if (_isPaused)
        {
            SetPause(false);
        }

        GameDataManager.DeleteSave();
        _sceneChanger.LoadCurrent();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        _sceneChanger.LoadByName("MenuScene");
    }

    private void OnBrickBrokeDown(Brick brick)
    {
        brick.BrokeDown -= OnBrickBrokeDown;

        brick.transform.SetParent(null);
        _bricksPool.ReturnObject(brick);
        ScoreManager.Instance.AddBrickDestroyCount();

        BrickDestroyParticles brickDestroyParticles = _brickParticlesPool.GetObject();
        brickDestroyParticles.transform.position = brick.transform.position;
        brickDestroyParticles.ParticlesPlayed += OnBrickDestroyParticlesPlayed;
    }

    private void OnBrickDestroyParticlesPlayed(BrickDestroyParticles brickDestroyParticles)
    {
        brickDestroyParticles.ParticlesPlayed -= OnBrickDestroyParticlesPlayed;
        _brickParticlesPool.ReturnObject(brickDestroyParticles);
    }

    private void OnRowsAllBricksBrokeDown(Row row)
    {
        _rows.Remove(row);
        _rowsPool.ReturnObject(row);
    }

    private void OnPickupableBallPicked(PickupableItem pickupableBall)
    {
        pickupableBall.Picked -= OnPickupableBallPicked;
        _pickupableBallPool.ReturnObject(pickupableBall);

        _pickedBallsCount++;
    }

    private void OnPickupableCoinPicked(PickupableItem pickupableCoin)
    {
        pickupableCoin.Picked -= OnPickupableCoinPicked;
        _pickupableCoinPool.ReturnObject(pickupableCoin);

        _pickedCoinsCount++;
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

        _showSpeedUpButtonCoroutine = StartCoroutine(ShowSpeedUpButton());
    }

    private void OnBallsFallen()
    {
        if (!_isPaused)
        {
            Time.timeScale = 1f;
        }

        _speedUpButton.gameObject.SetActive(false);

        if (_showSpeedUpButtonCoroutine != null)
        {
            StopCoroutine(_showSpeedUpButtonCoroutine);
        }

        _ballLauncher.BallsFallen -= OnBallsFallen;

        _ballLauncher.SpawnBall(_pickedBallsCount);
        _pickedBallsCount = 0;

        ScoreManager.Instance.AddBrickMove();

        PlayMoveRowsAnimation()
            .OnComplete(() =>
            {
                if (IsLosed())
                {
                    int score = ScoreManager.Instance.BrickDestroyCount;

                    if (score > PlayerDataManager.LoadPlayerData().BestScore)
                    {
                        PlayerDataManager.SavePlayerData(new PlayerData(score));
                    }

                    if (_pickedCoinsCount > 0)
                    {
                        PlayerData playerData = new PlayerData();
                        playerData.CoinsCount = PlayerDataManager.LoadPlayerData().CoinsCount + _pickedCoinsCount;

                        PlayerDataManager.SavePlayerData(playerData);
                    }

                    GameDataManager.DeleteSave();

                    _gameOverPanel.Appear();
                }
                else
                {
                    SpawnRow();
                    SaveGame();

                    _isCanLaunchBalls = true;
                    _ballLauncher.LaunchStarted += OnLaunchStarted;
                }
            });
    }

    private bool IsLosed()
    {
        foreach (Row row in _rows)
        {
            if (row.transform.position.y <= -3.5f)
            {
                return true;
            }
        }

        return false;
    }

    public void SetPause(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;

            _pausePanel.Appear();
        }
        else
        {
            Time.timeScale = 1f;
        }
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

    private void OnPauseButtonClicked()
    {
        SetPause(true);
    }

    private IEnumerator ShowSpeedUpButton()
    {
        yield return new WaitForSeconds(5f);

        _speedUpButton.gameObject.SetActive(true);
        _speedUpButton.onClick.AddListener(OnSpeedUpButtonClicked);
    }

    private void OnSpeedUpButtonClicked()
    {
        _speedUpButton.onClick.RemoveListener(OnSpeedUpButtonClicked);
        _speedUpButton.gameObject.SetActive(false);

        Time.timeScale = 2f;
    }

    public int PickedCoinsCount => _pickedCoinsCount;
}