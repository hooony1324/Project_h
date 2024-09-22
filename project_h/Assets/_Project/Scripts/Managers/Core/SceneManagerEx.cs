using System;
using System.Collections;
using System.Collections.Generic;
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

    public void LoadScene(EScene type, bool isAsync = true)
    {
        Managers.Clear();

        if (isAsync)
        {
            SceneManager.LoadSceneAsync(GetSceneName(type));
        }
        else
        {
            SceneManager.LoadScene(GetSceneName(type));
        }
    }

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
