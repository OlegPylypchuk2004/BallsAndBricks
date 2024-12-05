using UnityEngine;
using UnityEngine.UI;

public class ChooseBallButton : MonoBehaviour
{
    [SerializeField] private Image _ballImage;

    private int _number;
    private Color _color;
    private bool _isPurchaused;

    public void Initialize(int number, Color color, bool isPurchaused)
    {
        _number = number;
        _color = color;
        _isPurchaused = isPurchaused;

        _ballImage.color = _color;
    }
}