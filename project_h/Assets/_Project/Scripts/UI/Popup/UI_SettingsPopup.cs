using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_SettingsPopup : UI_Popup
{
    enum GameObjects
    {
        Layout_LocalizationButtons,
    }

    enum Texts
    {
        LanguageText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindGameObjects(typeof(GameObjects));
        BindTMPTexts(typeof(Texts));
        
        GameObject localizationButtons = GetGameObject((int)GameObjects.Layout_LocalizationButtons);
        GameObject languageButton = localizationButtons.transform.GetChild(0).gameObject;
        
        foreach (Transform child in localizationButtons.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        int index = 0;
        foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
        {
            UI_LanguageButton button = Managers.UI.MakeSubItem<UI_LanguageButton>(localizationButtons.transform);
            button.SetInfo(index, locale);
            index++;
        }

        return true;
    }

    // foreach (var locale in LocalizationSettings.AvailableLocales.Locales)


    // Localization Test

}