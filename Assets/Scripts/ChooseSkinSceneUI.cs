using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkinSceneUI : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private Button _backButton;
    [SerializeField] private ChooseBallButton[] _chooseBallButtons;
    
    private Gradient _ballsColorGradient;
    private int _chosenBallSkinIndex;

    private void Start()
    {
        PlayerData playerData = PlayerDataManager.LoadPlayerData();
        _chosenBallSkinIndex = playerData.ChosenBallSkinIndex;

        if (_chosenBallSkinIndex < 0 || _chosenBallSkinIndex > _chooseBallButtons.Length - 1)
        {
            _chosenBallSkinIndex = 0;
        }

        _ballsColorGradient = Resources.Load<BallsColorGradientData>("BallsColorGradient").Gradient;

        for (int i = 0; i < _chooseBallButtons.Length; i++)
        {
            Color targetColor = Color.white;

            if (i == 0)
            {
                _chooseBallButtons[i].Initialize(i + 1, targetColor, true, i == _chosenBallSkinIndex);
            }
            else
            {
                float t = Mathf.Clamp01((float)i / (_chooseBallButtons.Length - 1));
                targetColor = _ballsColorGradient.Evaluate(t);

                _chooseBallButtons[i].Initialize(i + 1, targetColor, playerData.PurchausedBallSkinIndexes.Contains(i), i == _chosenBallSkinIndex);
            }
        }
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);

        foreach (ChooseBallButton chooseBallButton in _chooseBallButtons)
        {
            chooseBallButton.Clicked += OnChooseBallButtonClicked;
        }
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);

        foreach (ChooseBallButton chooseBallButton in _chooseBallButtons)
        {
            chooseBallButton.Clicked -= OnChooseBallButtonClicked;
        }
    }

    private void OnBackButtonClicked()
    {
        _sceneChanger.LoadByName("MenuScene");
    }

    private void OnChooseBallButtonClicked(ChooseBallButton chooseBallButton)
    {
        PlayerData playerData = PlayerDataManager.LoadPlayerData();

        _chooseBallButtons[_chosenBallSkinIndex].Unselect();
        _chosenBallSkinIndex = Array.IndexOf(_chooseBallButtons, chooseBallButton);
        _chooseBallButtons[_chosenBallSkinIndex].Select();

        playerData.ChosenBallSkinIndex = _chosenBallSkinIndex;
        PlayerDataManager.SavePlayerData(playerData);

        if (!playerData.PurchausedBallSkinIndexes.Contains(_chosenBallSkinIndex) && CoinsManager.Spend(25))
        {
            playerData.PurchausedBallSkinIndexes.Add(_chosenBallSkinIndex);
        }
    }
}