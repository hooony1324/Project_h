using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.TMP_Dropdown;

public class UI_GameScene : UI_Scene
{

    enum Buttons
    {
        TestSceneButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.TestSceneButton).gameObject.BindEvent(OnClickTestSceneButton);

        return true;
    }

    void OnClickTestSceneButton()
    {
        SceneManager.LoadScene("LightTestScene");
    }
}
