using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public static class StringTable
{
    public static string GetWord(string key)
    {
        return Managers.Locale.GetWordString(key);
    }
    public static string GetMessage(string key)
    {
        return Managers.Locale.GetMessageString(key);
    }
}

public static class AssetTable
{
    public static T Get<T>(string key) where T : Object
        => Managers.Locale.GetAsset<T>(key);
    public static T Get<T>(string key, Locale locale) where T : Object
        => Managers.Locale.GetAsset<T>(key, locale);
}

public class LocaleManager
{
    public string GetWordString(string key) 
        => LocalizationSettings.StringDatabase.GetLocalizedString("WordTable", key, LocalizationSettings.SelectedLocale);


    public string GetMessageString(string key)
        => LocalizationSettings.StringDatabase.GetLocalizedString("MessageTable", key, LocalizationSettings.SelectedLocale);

    public T GetAsset<T>(string key) where T : Object
        => LocalizationSettings.AssetDatabase.GetLocalizedAsset<T>("AssetTable", key, LocalizationSettings.SelectedLocale);

    public T GetAsset<T>(string key, Locale locale) where T : Object
        => LocalizationSettings.AssetDatabase.GetLocalizedAsset<T>("AssetTable", key, locale);
}