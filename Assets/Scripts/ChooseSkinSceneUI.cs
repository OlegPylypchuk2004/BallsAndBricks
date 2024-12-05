using UnityEngine;
using UnityEngine.UI;

public class ChooseSkinSceneUI : MonoBehaviour
{
    [SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private Button _backButton;
    [SerializeField] private ChooseBallButton[] _chooseBallButtons;
    [SerializeField] private Gradient _ballsColorGradient;

    private void Start()
    {
        _ballsColorGradient = Resources.Load<BallsColorGradientData>("BallsColorGradient").Gradient;

        for (int i = 0; i < _chooseBallButtons.Length; i++)
        {
            Color targetColor = Color.white;

            if (i == 0)
            {
                _chooseBallButtons[i].Initialize(i + 1, targetColor, false);
            }
            else
            {
                float t = Mathf.Clamp01((float)i / (_chooseBallButtons.Length - 1));
                targetColor = _ballsColorGradient.Evaluate(t);

                _chooseBallButtons[i].Initialize(i + 1, targetColor, false);
            }
        }
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        _sceneChanger.LoadByName("MenuScene");
    }
}