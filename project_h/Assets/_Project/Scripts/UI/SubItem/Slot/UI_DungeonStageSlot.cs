using UnityEngine;

public class UI_DungeonStageSlot : UI_SubItem
{
    enum Texts
    {
        DungeonNameText,
    }

    enum Buttons
    {
        StageButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        
        GetButton((int)Buttons.StageButton).gameObject.BindEvent(OnClickDungeon);
        GetButton((int)Buttons.StageButton).gameObject.BindEvent(null, OnBeginDrag, EUIEvent.BeginDrag);
        GetButton((int)Buttons.StageButton).gameObject.BindEvent(null, OnDrag, EUIEvent.Drag);
        GetButton((int)Buttons.StageButton).gameObject.BindEvent(null, OnEndDrag, EUIEvent.EndDrag);

        return true;
    }


    private DungeonData _data;
    public void SetInfo(DungeonData data)
    {
        SetParentScrollRect();

        GetTMPText((int)Texts.DungeonNameText).text = data.DungeonName;
        _data = data;
    }

    
    private void OnClickDungeon()
    {
        UI_MessagePopup messagePopup = Managers.UI.ShowPopupUI<UI_MessagePopup>();
        messagePopup.SetInfo(
            "던전에 입장하시겠습니까?",
            OnClickDecision,
            showConfirmButton: true,
            showCancelButton: true);
    }

    private void OnClickDecision(bool result)
    {
        if (result)
        {
            Managers.Map.LoadMap(_data.PrefabName, new Vector3(100, -100, 0));
            Managers.UI.CloseAllPopupUI();
        }
    }
}