using System;
using UnityEngine.UI;


public class UI_MessagePopup : UI_Popup
{
    enum Texts
    {
        TitleText,
        MessageText,

        ConfirmText,
        CancelText,
    }
    enum Buttons
    {
        ConfirmButton,      // left Button
        CancelButton,       // right Button
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
        
        GetTMPText((int)Texts.TitleText).gameObject.SetActive(false);
        return true;
    }

    /// <summary>
    ///   "~~하시겠습니까"
    ///     yes     no
    /// </summary>
    public void SetupMessageConfrim(string msgText, Action<bool> callback = null,
    bool showConfirmButton = true, bool showCancelButton = true,
    string confirmButtonText = null, string cancelButtonText = null)
    {
        GetTMPText((int)Texts.MessageText).text = msgText;

        Button confirmButton = GetButton((int)Buttons.ConfirmButton);
        confirmButton.gameObject.SetActive(showConfirmButton);        
        GetTMPText((int)Texts.ConfirmText).text = confirmButtonText ?? StringTable.GetWord("Confirm");

        Button cancelButton = GetButton((int)Buttons.CancelButton);
        cancelButton.gameObject.SetActive(showCancelButton);
        GetTMPText((int)Texts.CancelText).text = cancelButtonText ?? StringTable.GetWord("Cancel");

        if (callback != null)
            OnDecision = callback;
    }

    public void SetupTitleAndMessageConfirm(string titleText, string msgText, Action<bool> callback = null,
    bool showConfirmButton = true, string confirmButtonText = null,
    bool showCancelButton = true, string cancelButtonText = null)
    {
        GetText((int)Texts.TitleText).gameObject.SetActive(true);
        GetTMPText((int)Texts.TitleText).text = titleText;

        SetupMessageConfrim(msgText, callback, showConfirmButton, showCancelButton, confirmButtonText, cancelButtonText);
    }
    
    private void OnConfirmButtonClicked()
    {
        OnDecision?.Invoke(true);
        base.ClosePopupUI();
    }

    private void OnCancelButtonClicked()
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
