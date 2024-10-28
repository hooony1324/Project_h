using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
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
    //private class LocalizingMonoBehaviour : MonoBehaviour { }
    public void Init()
    {
        // GameObject loadingGameObject = new GameObject("@Loading Game Object");
        // loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            Debug.Log(locale.LocaleName);
            // Korean (ko), Japanese (ja), English (en)
            // 이걸로 settingsUI에서 동적으로 Flag버튼 생성
            // 시트로 출력 -> GPT에서 자동번역 되는지 여부 확인

            // 코드로 세팅하는 방법
            //Locale cuurrentLocale = LocalizationSettings.SelectedLocale;
            //LocalizationSettings.StringDatabase.GetLocalizedString("MyTableName", "key", cuurrentLocale);
        }
    }

    public string GetWordString(string key) 
        => LocalizationSettings.StringDatabase.GetLocalizedString("WordTable", key, LocalizationSettings.SelectedLocale);


    public string GetMessageString(string key)
        => LocalizationSettings.StringDatabase.GetLocalizedString("MessageTable", key, LocalizationSettings.SelectedLocale);

    public T GetAsset<T>(string key) where T : Object
        => LocalizationSettings.AssetDatabase.GetLocalizedAsset<T>("AssetTable", key, LocalizationSettings.SelectedLocale);

    public T GetAsset<T>(string key, Locale locale) where T : Object
        => LocalizationSettings.AssetDatabase.GetLocalizedAsset<T>("AssetTable", key, locale);

    // 이벤트 방식 동적 변환 예제
    // ex) Dialogue 떠있는데 언어 변경
    // [다른 클래스에]

    // private void Init()
    // {
    //     LocalizationSettings.SelectedLocaleChanged += OnChangeLocale;
    // }
    
    // void OnChangeLocale(Locale locale)
    // {
    //     StartCoroutine(ChangeLocaleRoutine(locale));
    // }

    // private IEnumerator ChangeLocaleRoutine(Locale locale)
    // {
    //     var asyncOperation = LocalizationSettings.StringDatabase.GetTableAsync("MyTable");
        
    //     yield return asyncOperation;
        
    //     if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
    //     {
    //         StringTable table = asyncOperation.Result;
            
    //         string tableValue = table.GetEntry("Key")?.GetLocalizedString();
    //         // content.text = tableValue;
    //     }
    // }
    
}