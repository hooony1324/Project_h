using UnityEngine;

public class LoadingScene : BaseScene
{
    private bool _isFirstUpdate = true;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.LoadingScene;
        Managers.Scene.SetCurrentScene(this);

        Managers.UI.ShowSceneUI<UI_LoadingScene>();

        return true;
    }

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            Managers.Scene.LoaderCallBack();
        }
    }

    public override void Clear() { }

}