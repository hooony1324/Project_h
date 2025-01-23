using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Scene
{
    enum GameObjects
    {
        MiddleLayout,
        RemoveCacheButton,
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
        GetGameObject((int)GameObjects.MiddleLayout).SetActive(false);
        GetGameObject((int)GameObjects.RemoveCacheButton).SetActive(false);

        TitleScene titleScene = Managers.Scene.GetCurrentScene<TitleScene>();
        titleScene.OnDownloadEnd += HandleDownloadEnded;
        titleScene.OnDownloadStateStateChanged += HandleDownloadStateChanged;

        #if DEBUGCODE
        GetGameObject((int)GameObjects.RemoveCacheButton).SetActive(true);
        GetGameObject((int)GameObjects.RemoveCacheButton).BindEvent(
            () => 
            {
                Managers.SaveLoad.RemovePlayData();
            }
        );
        #endif 

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
