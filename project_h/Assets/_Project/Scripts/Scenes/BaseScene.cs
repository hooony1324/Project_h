using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EScene
{
    Unknown,
    LoadingScene,
    TitleScene,
    SelectHeroScene,
    GameScene,
    DungeonScene,
}

public abstract class BaseScene : InitOnce
{
    public EScene SceneType { get; protected set; } = EScene.Unknown;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        

        return true;
    }

    public abstract void Clear();
}
