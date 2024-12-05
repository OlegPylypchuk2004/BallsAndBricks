using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSoundPlayer : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioClip _clickSound;

    private void OnValidate()
    {
        _button ??= GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SoundManager.Instance.PlayAudioClip(_clickSound);
    }
}