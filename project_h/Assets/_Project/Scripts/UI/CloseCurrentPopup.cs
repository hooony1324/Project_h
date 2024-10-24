
using UnityEngine;

public class CloseCurrentPopup : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindEvent
        BindEvent(gameObject, ClosePopup);

        return true;
    }

    void ClosePopup()
    {
        Managers.UI.ClosePopupUI(); 
    }
}