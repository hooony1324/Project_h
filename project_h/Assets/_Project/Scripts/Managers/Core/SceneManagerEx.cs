using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    private BaseScene _currentScene;

    public T GetCurrentScene<T>() where T : BaseScene
    {
        return _currentScene as T;
    }

    public void SetCurrentScene(BaseScene scene)
    {
        _currentScene = scene;
    }


#region Loading...
    private Action _onLoaderCallback;
    public Action<float> OnLoadingProgress;
    private class LoadingMonoBehaviour : MonoBehaviour { }
    public void LoadScene(EScene scene, bool isUseLoadingScene = true)
    {
        Managers.Clear();

        // LoadingScene없이 진행
        if (!isUseLoadingScene)
        {
            SceneManager.LoadSceneAsync(GetSceneName(scene));
            return;
        }
        
        // LoadingScene으로 진행
        _onLoaderCallback = () => 
        {
            GameObject loadingGameObject = new GameObject("@Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene(GetSceneName(EScene.LoadingScene));
    }

    private IEnumerator LoadSceneAsync(EScene scene)
    {
        OnLoadingProgress?.Invoke(0);
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GetSceneName(scene));
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            OnLoadingProgress?.Invoke(asyncOperation.progress);
            yield return WaitFor.Seconds(0.4f);
        }

        OnLoadingProgress?.Invoke(1f);
        yield return WaitFor.Seconds(1);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void LoaderCallBack()
    {
        if (_onLoaderCallback != null)
        {
            _onLoaderCallback.Invoke();
            _onLoaderCallback = null;
        }
    }
#endregion

    public bool CheckCurrentScene(EScene type)
    {
        return SceneManager.GetActiveScene().name.Equals(GetSceneName(type));   
    }
    private string GetSceneName(EScene type)
    {
        string name = Enum.GetName(typeof(EScene), type);
        return name;
    }

    public void Clear()
    {
        _currentScene.Clear();
    }
}
