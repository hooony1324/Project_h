using static Define;

public class UI_ItemSelectorSlot : UI_Base
{
    enum Images
    {
        ItemSprite,
    }

    enum Texts
    {
        DescriptionText,
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));
        BindTMPTexts(typeof(Texts));

        gameObject.BindEvent(OnClickSlot);

        return true;
    }

    int selectSkillID;
    int resultSkillID;
    EDefaultSkillSlot skillSlot;

    public void Setup(ItemSelectorInfo info)
    {
        this.selectSkillID = info.selectSkillID;
        this.resultSkillID = info.resultSkillID;

        SetupDefaultSkill();
    }   

    void OnClickSlot()
    {
        Managers.UI.ClosePopupUI();

        Managers.Hero.ChangeDefaultSkill(resultSkillID, skillSlot);
    }

    void SetupDefaultSkill()
    {
        Skill skill = Managers.Data.GetSkillData(resultSkillID);
        
        skill.Level = 1;

        GetImage((int)Images.ItemSprite).sprite = skill.Icon;
        GetTMPText((int)Texts.DescriptionText).text = skill.Description;
    }
}