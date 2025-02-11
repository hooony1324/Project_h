using UnityEngine;


// 최대 체력 5 => 하트 2개 반
// 최대 체력 10 = > 하트 5개
public class PlayContinue : UI_Base 
{
    enum GameObjects
    {
        Button_Continue,
        Button_Lobby,
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindGameObjects(typeof(GameObjects));
        GetGameObject((int)GameObjects.Button_Lobby).BindEvent(OnClickLobby);
        GetGameObject((int)GameObjects.Button_Continue).BindEvent(OnClickContinue);

        return true;
    }

    void OnClickLobby()
    {
        Managers.Game.GoToLobby();
    }

    void OnClickContinue()
    {
        Managers.Game.ReviveHero();
    }

}