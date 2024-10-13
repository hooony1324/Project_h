using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;

/// <summary>
/// Version : 1.2.3
/// 1 : Major Version, 틀이 바뀌는 대규모 업데이트
/// 2 : Minor Version, 기존 버전에 새로운 기능이 추가 된 버전
/// 3 : Patch(Build), 자잘한 버그를 수정한 버전
/// 
/// 버전 확인
/// Debug.Log("Build" + Application.version);
/// </summary>
[InitializeOnLoad]
public class VersionManager
{
    private static bool autoIncrease = true;

    private const string AutoIncreaseMenuName = "Build/Auto Increase Build Version";

    static VersionManager()
    {
        autoIncrease = EditorPrefs.GetBool(AutoIncreaseMenuName, true);
    }

    [MenuItem(AutoIncreaseMenuName, false, 1)]
    private static void SetAutoIncrease()
    {
        autoIncrease = !autoIncrease;
        EditorPrefs.SetBool(AutoIncreaseMenuName, autoIncrease);

        Debug.Log("Auto Increase :" + autoIncrease);
    }

    [MenuItem(AutoIncreaseMenuName, true)]
    private static bool SetAutoIncreaseValidate()
    {
        Menu.SetChecked(AutoIncreaseMenuName, autoIncrease);
        return true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    [MenuItem("Build/Check Current Version", false, 2)]
    private static void CheckCurrentVersion()
    {
        Debug.Log($"Build Version: {PlayerSettings.bundleVersion}({PlayerSettings.Android.bundleVersionCode})");
    }

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (autoIncrease)
            IncreaseBuild();
    }

    [MenuItem("Build/Increase Major Version", false, 51)]
    private static void IncreaseMajor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(1, -int.Parse(lines[1]), -int.Parse(lines[2]));
    }

    [MenuItem("Build/Increase Minor Version", false, 52)]
    private static void IncreaseMinor()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        EditVersion(0, 1, -int.Parse(lines[2]));
    }

    private static void IncreaseBuild()
    {
        EditVersion(0, 0, 1);
    }

    static void EditVersion(int majorIncr, int minorIncr, int buildIncr)
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int majorVersion = int.Parse(lines[0]) + majorIncr;
        int minorVersion = int.Parse(lines[1]) + minorIncr;
        int buildVersion = int.Parse(lines[2]) + buildIncr;

        PlayerSettings.bundleVersion = $"{majorVersion}.{minorVersion}.{buildVersion}";
        PlayerSettings.Android.bundleVersionCode = majorVersion * 10000 + minorVersion * 1000 + buildVersion;
        CheckCurrentVersion();
    }

    // 0.1(최초) => 0.0.0
    [InitializeOnLoadMethod]
    static void CheckVersionLength()
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');
        if (lines.Length < 3)
        {
            int majorVersion = 0;
            int minorVersion = 0;
            int buildVersion = 0;

            PlayerSettings.bundleVersion = $"{majorVersion}.{minorVersion}.{buildVersion}";
            PlayerSettings.Android.bundleVersionCode = majorVersion * 10000 + minorVersion * 1000 + buildVersion;
        }
    }
}
#endif