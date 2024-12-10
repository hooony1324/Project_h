using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class SelectHeroScene : BaseScene
{


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.SelectHeroScene;
        Managers.Scene.SetCurrentScene(this);


        Managers.UI.ShowSceneUI<UI_SelectHeroScene>();

        return true;
    }

    void Start()
    {

    }

    public override void Clear()
    {

    }
}