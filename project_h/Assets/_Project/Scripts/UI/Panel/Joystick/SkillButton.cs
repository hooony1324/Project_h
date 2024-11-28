using UnityEngine;

public abstract class SkillButton : UI_Base
{
    private Entity _owner;
    private Skill _skill;

    protected Skill Skill => _skill;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        gameObject.BindEvent(OnClickButton);

        return true;
    }

    public virtual void BindSkill(Entity owner, Skill skill)
    {
        _owner = owner;
        _skill = skill;
    }

    public virtual void UnBindSkill()
    {
        _skill = null;
    }

    protected virtual void OnClickButton()
    {
        if (_skill != null && _skill.IsUseable)
        {
            if (_owner.Target == null)
                return;
                
            _owner.SkillSystem.CancelAll();
            _skill.Use();
        }
    }
}
