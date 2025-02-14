using UnityEngine;


public class Joystick_SkillButton_Attack : SkillButton
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    


        return true;
    }
    public override void BindSkill(Entity owner, Skill skill)
    {
        base.BindSkill(owner, skill);
    }

    public override void UnBindSkill()
    {
        base.UnBindSkill();
    }


}
