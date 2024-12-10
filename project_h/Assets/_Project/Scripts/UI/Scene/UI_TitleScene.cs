using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Scene
{
    enum GameObjects
    {
        MiddleLayout,
    }

    enum Texts
    {
        LoadingStatusText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindGameObjects(typeof(GameObjects));
        BindTMPTexts(typeof(Texts));

        GetGameObject((int)GameObjects.MiddleLayout).gameObject.BindEvent(OnClickMiddleLayout);
        GetGameObject((int)GameObjects.MiddleLayout).gameObject.SetActive(false);

        TitleScene titleScene = Managers.Scene.GetCurrentScene<TitleScene>();
        titleScene.OnDownloadEnd += HandleDownloadEnded;
        titleScene.OnDownloadStateStateChanged += HandleDownloadStateChanged;

        return true;
    }

    void HandleDownloadEnded()
    {
        GetGameObject((int)GameObjects.MiddleLayout).gameObject.SetActive(true);
    }

    void HandleDownloadStateChanged(string message)
    {
        GetTMPText((int)Texts.LoadingStatusText).text = message;
    }

    void OnClickMiddleLayout()
    {
        Managers.Scene.LoadScene(EScene.SelectHeroScene, false);
    }

    private void OnDisable()
    {
        TitleScene titleScene = Managers.Scene.GetCurrentScene<TitleScene>();
        titleScene.OnDownloadEnd -= HandleDownloadEnded;
        titleScene.OnDownloadStateStateChanged -= HandleDownloadStateChanged;
    }
}
