using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public string sceneToLoad = "GameScene";
    public float minimumShowTime = 2f; // so it doesn't flash by on fast loads

    IEnumerator Start()
    {
        float startTime = Time.unscaledTime;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        op.allowSceneActivation = false;

        // Unity reports 0.9 when the scene is ready but not yet activated
        while (op.progress < 0.9f || Time.unscaledTime - startTime < minimumShowTime)
            yield return null;

        op.allowSceneActivation = true;
    }
}