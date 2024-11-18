using UnityEngine;

public class Joystick_ActionButtons : InitOnce
{
    [SerializeField] private Joystick_SkillButton_Attack _attackButton;
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

        _rollButton.Setup( _owner.SkillSystem.Roll, _owner.Roll);
    }
}
