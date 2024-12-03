using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Image _fadeImage;

    private AsyncOperation _loadSceneOperation;

    private IEnumerator Start()
    {
        PlayDisappearAnimation();

        yield return new WaitForSeconds(0.25f);

        _eventSystem.gameObject.SetActive(true);
    }

    public void LoadByName(string sceneName)
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadSceneOperation.allowSceneActivation = false;

        StartCoroutine(DelayBeforeLoad());
    }

    public void LoadByIndex(int sceneIndex)
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
        _loadSceneOperation.allowSceneActivation = false;

        StartCoroutine(DelayBeforeLoad());
    }

    public void LoadCurrent()
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        _loadSceneOperation.allowSceneActivation = false;

        StartCoroutine(DelayBeforeLoad());
    }

    public void LoadPrevious()
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        _loadSceneOperation.allowSceneActivation = false;

        StartCoroutine(DelayBeforeLoad());
    }

    public void LoadNext()
    {
        _loadSceneOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        _loadSceneOperation.allowSceneActivation = false;

        StartCoroutine(DelayBeforeLoad());
    }

    private IEnumerator DelayBeforeLoad()
    {
        _eventSystem.gameObject.SetActive(false);

        PlayAppearAnimation();

        yield return new WaitForSeconds(0.5f);

        _loadSceneOperation.allowSceneActivation = true;
    }

    private void PlayAppearAnimation()
    {
        _fadeImage.DOFade(1f, 0.25f)
            .From(0f)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject)
            .SetUpdate(true);
    }

    private void PlayDisappearAnimation()
    {
        _fadeImage.DOFade(0f, 0.25f)
            .From(1f)
            .SetEase(Ease.InQuad)
            .SetLink(gameObject)
            .SetUpdate(true);
    }
}