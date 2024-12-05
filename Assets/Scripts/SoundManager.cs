using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public static SoundManager Instance { get; private set; }

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

    public void PlayAudioClip(AudioClip audioClip)
    {
        if (PlayerDataManager.LoadPlayerData().IsSoundDisabled)
        {
            return;
        }

        _audioSource.PlayOneShot(audioClip);
    }
}