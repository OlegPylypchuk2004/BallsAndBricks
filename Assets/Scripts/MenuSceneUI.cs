using TMPro;
using UnityEngine;

public class MenuSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private void Start()
    {
        _bestScoreText.text = $"{PlayerDataManager.LoadPlayerData().BestScore}";
    }
}