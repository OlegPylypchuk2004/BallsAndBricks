using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBallButton : MonoBehaviour
{
    [SerializeField] private Image _ballImage;
    [SerializeField] private Image _outlineImage;
    [SerializeField] private Button _button;

    private int _number;
    private Color _color;
    private bool _isPurchaused;

    public event Action<ChooseBallButton> Clicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    public void Initialize(int number, Color color, bool isPurchaused, bool isSelected = false)
    {
        _number = number;
        _color = color;
        _isPurchaused = isPurchaused;

        _ballImage.color = _color;
        _button.interactable = isPurchaused;

        Color targetOutlineColor = Color.white;

        if (isSelected)
        {

        }
        else
        {
            targetOutlineColor.a = .5f;
        }

        _outlineImage.color = targetOutlineColor;
    }

    public void Select()
    {
        Color targetOutlineColor = Color.white;

        _outlineImage.color = targetOutlineColor;
    }

    public void Unselect()
    {
        Color targetOutlineColor = Color.white;
        targetOutlineColor.a = .5f;

        _outlineImage.color = targetOutlineColor;
    }

    private void OnButtonClicked()
    {
        Clicked?.Invoke(this);
    }
}