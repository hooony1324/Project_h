using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MessagePopup : UI_Popup
{
    enum Texts
    {
        MessageText,

        ConfirmText,
        CancelText,
    }
    enum Buttons
    {
        ConfirmButton,      // left Button
        CancelButton,       // right Button
        ExitButton,
    }

    private Action<bool> OnDecision;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTMPTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnConfirmButtonClicked);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnCancelButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(ClosePopupUI);

        return true;
    }

    
    public void SetInfo(string msgText, Action<bool> callback = null,
    bool showConfirmButton = false, string confirmButtonText = null,
    bool showCancelButton = false, string cancelButtonText = null,
    bool showExitButton = false)
    {
        GetTMPText((int)Texts.MessageText).text = msgText;

        Button confirmButton = GetButton((int)Buttons.ConfirmButton);
        confirmButton.gameObject.SetActive(showConfirmButton);        
        GetTMPText((int)Texts.ConfirmText).text = confirmButtonText ?? "확인";

        Button cancelButton = GetButton((int)Buttons.CancelButton);
        cancelButton.gameObject.SetActive(showCancelButton);
        GetTMPText((int)Texts.CancelText).text = cancelButtonText ?? "취소";

        if (callback != null)
            OnDecision = callback;

        GetButton((int)Buttons.ExitButton).gameObject.SetActive(showExitButton);
    }

    private void OnConfirmButtonClicked(PointerEventData eventData)
    {
        OnDecision?.Invoke(true);
        base.ClosePopupUI();
    }

    private void OnCancelButtonClicked(PointerEventData eventData)
    {
        OnDecision?.Invoke(false);
        base.ClosePopupUI();
    }

    private void OnDisable()
    {
        OnDecision = null;
    }

    private void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}
