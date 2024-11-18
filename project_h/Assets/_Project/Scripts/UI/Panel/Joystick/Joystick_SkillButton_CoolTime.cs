using System;
using UnityEngine;

public class Joystick_SkillButton_CoolTime : UI_Base
{
    private Skill _skill;
    private Action _skillAction = () => {};

    enum Sliders
    {
        RadialSlider,
    }

    enum Images
    {
        SkillIcon,
    }

    enum TMPTexts
    {
        CooltimeText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        BindSliders(typeof(Sliders));
        BindImages(typeof(Images));
        BindTMPTexts(typeof(TMPTexts));

        gameObject.BindEvent(OnClickButton);

        return true;
    }

    public void Setup(Skill skill, Action skillAction)
    {
        if (_skill)
            _skill.onStateChanged -= OnSkillStateChanged;
        
        _skill = skill;
        _skillAction = skillAction;

        
    }

    private void OnClickButton()
    {
        _skill.Use();
    }

    private void Update()
    {

    }

    private void OnSkillStateChanged(Skill skill, State<Skill> newState, State<Skill> prevState, int layer)
    {

    }
}
