using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

[System.Serializable]
public class SelectDefaultSkill : ItemAcquireAction
{
    [SerializeField] private EDefaultSkillSlot slotType;
    public override bool IsSpawnable => GetSkillFusionDatas(slotType).Count() > 0;

    public override void AqcuireAction(Item owner) 
    {
        this.owner = owner;

        var skillFusionDatas = GetSkillFusionDatas(slotType);
        
        if (skillFusionDatas.Count == 1)
        {
            Managers.Hero.ChangeDefaultSkill(skillFusionDatas[0].resultSkillID, slotType);
        }
        else
        {
            ItemSelectorInfo[] selectorInfos = skillFusionDatas.Select(data =>
            new ItemSelectorInfo
            {
                selectSkillID = data.targetSkillID,
                resultSkillID = data.resultSkillID,
            }).ToArray();

            UI_ItemSelectorPopup itemSelectorPopup = Managers.UI.ShowPopupUI<UI_ItemSelectorPopup>();
            itemSelectorPopup.Setup(selectorInfos);
        }
    }

    private IReadOnlyList<SkillFusionData> GetSkillFusionDatas(EDefaultSkillSlot slotType)
    {
        int sourceSkillID = 0;
        switch (slotType)
        {
            case EDefaultSkillSlot.DefaultAttack:
                sourceSkillID = Managers.Hero.MainHero.SkillSystem.DefaultAttack.ID;
                break;
            case EDefaultSkillSlot.Dodge:
                sourceSkillID = Managers.Hero.MainHero.SkillSystem.Dodge.ID;
                break;
        }

        return Managers.Data.GetSkillFusionDatas(sourceSkillID);
    }
}