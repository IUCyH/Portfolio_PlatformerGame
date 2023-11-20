using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LoadScene
{
    Title,
    Game,
    Loading
}

public class SceneLoadManager : Singleton_DontDestroy<SceneLoadManager>
{
    [SerializeField]
    Image progressBar;
    [SerializeField]
    Canvas loadingCanvas;
    AsyncOperation sceneShouldLoad;

    LoadScene sceneToLoad;
    bool loadNow;
    
    protected override void OnAwake()
    {
        loadingCanvas.enabled = false;
    }

    public void Load(LoadScene scene)
    {
        loadingCanvas.enabled = true;
        
        SceneManager.LoadSceneAsync((int)LoadScene.Loading);
        sceneToLoad = scene;
        loadNow = true;

        StartCoroutine(Coroutine_LoadScene());
    }

    IEnumerator Coroutine_LoadScene()
    {
        yield return null;
        float timer = 0f;
        
        while (true)
        {
            if (loadNow)
            {
                sceneShouldLoad = SceneManager.LoadSceneAsync((int)sceneToLoad);
                sceneShouldLoad.allowSceneActivation = false;
                loadNow = false;
            }

            if (PatchManager.Instance.PatchDone && SpriteTable.Instance.LoadDone)
            {
                if (sceneShouldLoad.progress < 0.9f)
                {
                    progressBar.fillAmount = sceneShouldLoad.progress;
                }
                else
                {
                    timer += Time.deltaTime;
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                    if (timer > 1f)
                    {
                        loadingCanvas.enabled = false;
                        sceneShouldLoad.allowSceneActivation = true;
                        
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }
}
