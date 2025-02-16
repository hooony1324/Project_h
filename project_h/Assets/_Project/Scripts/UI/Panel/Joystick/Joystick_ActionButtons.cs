using UnityEngine;

public class Joystick_ActionButtons : InitOnce
{
    [SerializeField] private Joystick_SkillButton_CoolTime _attackButton;
    [SerializeField] private Joystick_SkillButton_CoolTime _rollButton;

    private Entity _owner;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    

        return true;
    }

    public void Setup(Entity owner)
    {
        _owner = owner;

        _rollButton.UnBindSkill();
        _attackButton.UnBindSkill();

        _rollButton.BindSkill(_owner, _owner.SkillSystem.Dodge);
        _attackButton.BindSkill(_owner, _owner.SkillSystem.DefaultAttack);

    }


}
