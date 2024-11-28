using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private TextMeshPro _numberText;

    private int _number;

    private void Awake()
    {
        _number = Random.Range(1, 4);
        UpdateView();
    }

    public void Hit()
    {
        _number--;

        if (_number <= 0)
        {
            Destroy(gameObject);
        }

        UpdateView();
    }

    private void UpdateView()
    {
        _numberText.text = $"{_number}";
    }
}