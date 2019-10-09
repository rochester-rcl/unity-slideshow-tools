using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class SceneController : MonoBehaviour
{
    private int currentSceneIndex = 0;
    public bool useFade = false;
    // Use this for initialization
    private FullscreenFade FadeController;
    void Awake()
    {
		FadeController = gameObject.AddComponent<FullscreenFade>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneTimer.OnSceneDurationEnd += LoadNext;
    }

    void OnDisable()
    {
        if (useFade)
        {
            FullscreenFade.OnFadeOutEnds -= LoadNext;
        }
        else
        {
            SceneTimer.OnSceneDurationEnd -= LoadNext;
        }
    }

    void Start()
    {
        LoadNext();
    }

    public void LoadNext()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        currentSceneIndex++;
        AsyncOperation asyncLoader;
        if (SceneManager.sceneCountInBuildSettings > currentSceneIndex)
        {
            asyncLoader = SceneManager.LoadSceneAsync(currentSceneIndex);
        }
        else
        {
            currentSceneIndex = 1;
            asyncLoader = SceneManager.LoadSceneAsync(currentSceneIndex);
        }
        asyncLoader.allowSceneActivation = false;
        Coroutine fade;
        while (!asyncLoader.isDone)
        {
            if (asyncLoader.progress >= 0.9f)
            {
                fade = StartCoroutine(FadeAsync(false));
                yield return fade;
                asyncLoader.allowSceneActivation = true;
            }
            yield return null;
        }
		FadeController.UpdateCamera();
        Coroutine fadeIn = StartCoroutine(FadeAsync(true));
        yield return fadeIn;
    }

    private IEnumerator FadeAsync(bool fadeIn)
    {
        if (fadeIn)
        {
            Task t = FadeController.FadeInAsync();
            while (!t.IsCompleted)
            {
                yield return null;
            }
        }
        else
        {
            Task t = FadeController.FadeOutAsync();
            while (!t.IsCompleted)
            {
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
