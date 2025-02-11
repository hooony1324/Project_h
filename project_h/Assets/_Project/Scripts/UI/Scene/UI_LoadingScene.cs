using DG.Tweening;

public class UI_LoadingScene : UI_Scene
{
    enum Texts
    {
        LoadingText,
    }

    float _currentProgress;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));
        return true;
    }

    private void OnEnable()
    {
        Managers.Scene.OnLoadingProgress -= RefreshProgress;
        Managers.Scene.OnLoadingProgress += RefreshProgress;
    }
    private void OnDisable()
    {
        Managers.Scene.OnLoadingProgress -= RefreshProgress;
    }

    private void RefreshProgress(float targetProgress)
    {
        DOTween.To(() => _currentProgress, x =>
        {
            _currentProgress = x;
            string value = (_currentProgress * 100f).ToString("F1");
            GetTMPText((int)Texts.LoadingText).text = $"Loading . . . {value}%";
        }, targetProgress, 0.4f)
        .SetEase(Ease.OutCubic)
        .SetId("ProgressTween")
        .ChangeEndValue(targetProgress, true); // 목표 값 갱신
    }
}