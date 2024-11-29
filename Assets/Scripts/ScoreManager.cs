using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;

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

    public int Score
    {
        get { return _score; }
    }

    public void AddScore()
    {
        _score++;
    }
}