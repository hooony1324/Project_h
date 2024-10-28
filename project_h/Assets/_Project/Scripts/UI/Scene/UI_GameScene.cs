using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.TMP_Dropdown;

public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        SettingsButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));

        GetGameObject((int)GameObjects.SettingsButton).BindEvent(OnClickSettings);

        return true;
    }

    void OnClickSettings()
    {
        Managers.UI.ShowPopupUI<UI_SettingsPopup>();
    }
}
