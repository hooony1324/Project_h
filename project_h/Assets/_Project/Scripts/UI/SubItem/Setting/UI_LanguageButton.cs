using System.Collections;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine;

public class UI_LanguageButton : UI_SubItem
{
    private int _index;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        gameObject.BindEvent(OnClickButton);
        return true;
    }

    public void SetInfo(int index, Locale locale)
    {
        _index = index;
        GetComponent<Image>().sprite = AssetTable.Get<Sprite>("CountryFlag", locale); 
    }

    private void OnClickButton()
    {
        if (_isChanging)
            return;

        StartCoroutine(ChangeLocale());
    }

    private bool _isChanging;
    private IEnumerator ChangeLocale()
    {
        _isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_index];

        _isChanging = false;
    }
}
