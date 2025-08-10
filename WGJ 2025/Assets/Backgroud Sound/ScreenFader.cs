using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public void TransitionToScene(string sceneName, float duration)
    {
        StartCoroutine(DoTransition(sceneName, duration));
    }

    private IEnumerator DoTransition(string sceneName, float duration)
    {
        // fade out
        yield return FadeOut(duration);

        // load scene
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // fade back in
        yield return FadeIn(duration);
    }

    public static ScreenFader Instance { get; private set; }

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] bool fadeInOnStart = true;
    [SerializeField] float startFadeInDuration = 0.6f;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (!canvasGroup) canvasGroup = GetComponentInChildren<CanvasGroup>(true);

        // Start state
        if (fadeInOnStart)
        {
            canvasGroup.alpha = 1f;
            StartCoroutine(FadeIn(startFadeInDuration));
        }
        else canvasGroup.alpha = 0f;
    }

    public System.Collections.IEnumerator FadeOut(float duration)
        => FadeTo(1f, duration);

    public System.Collections.IEnumerator FadeIn(float duration)
        => FadeTo(0f, duration);

    System.Collections.IEnumerator FadeTo(float target, float duration)
    {
        if (!canvasGroup) yield break;
        float start = canvasGroup.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        canvasGroup.alpha = target;
    }
}
