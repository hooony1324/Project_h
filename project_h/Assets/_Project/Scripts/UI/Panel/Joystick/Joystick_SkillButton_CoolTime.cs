using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Joystick_SkillButton_CoolTime : SkillButton
{

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

        GetSlider((int)Sliders.RadialSlider).value = 0;
        GetImage((int)Images.SkillIcon).gameObject.SetActive(false);
        GetTMPText((int)TMPTexts.CooltimeText).gameObject.SetActive(false);

        return true;
    }

    public override void BindSkill(Entity owner, Skill skill)
    {
        if (Skill)
            Skill.onStateChanged -= OnSkillStateChanged;
        
        base.BindSkill(owner, skill);
        
        if (skill != null)
        {
            skill.onStateChanged += OnSkillStateChanged;

            Image skillImage = GetImage((int)Images.SkillIcon);
            skillImage.sprite = skill.Icon;
            GetImage((int)Images.SkillIcon).gameObject.SetActive(true);
        }
        else
        {
            // Deactivate button

        }
        
    }

    public override void UnBindSkill()
    {
        base.UnBindSkill();
        if (Skill != null)
        {
            Skill.onStateChanged -= OnSkillStateChanged;
            GetImage((int)Images.SkillIcon).gameObject.SetActive(false);
        }
    }

    // private void Update()
    // {
    //     if (!_skill)
    //         return;

    //     UpdateBlindImage();
    //     UpdateInput();
    // }

    private void OnSkillStateChanged(Skill skill, State<Skill> newState, State<Skill> prevState, int layer)
    {
        var stateType = newState.GetType();

        if (stateType == typeof(CooldownState))
            ShowCooldown();
        else if (stateType == typeof(InActionState))
            ShowAction();
    }

    private async Awaitable ShowCooldown()
    {
        Slider slider = GetSlider((int)Sliders.RadialSlider);
        TMP_Text cooltime = GetTMPText((int)TMPTexts.CooltimeText);

        slider.gameObject.SetActive(true);
        cooltime.gameObject.SetActive(true);

        if (Skill.ApplyCycle > 0f)
            slider.value = 1;
        
        while (Skill.IsInState<CooldownState>())
        {
            cooltime.text = Skill.CurrentCooldown.ToString("F1");
            slider.value = Skill.CurrentCooldown / Skill.Cooldown;

            await Awaitable.EndOfFrameAsync();
        }
        
        slider.value = 0;
        cooltime.gameObject.SetActive(false);
    }

    private async Awaitable ShowAction()
    {
        Slider slider = GetSlider((int)Sliders.RadialSlider);

        if (Skill.ApplyCycle > 0f)
            slider.value = 1;

        while (Skill.IsInState<InActionState>())
        {
            if (slider.gameObject.activeSelf)
                slider.value = 1f - (Skill.CurrentApplyCycle / Skill.ApplyCycle);
            
            // 리븐 Q 스킬(총 3번 누름) 1번 > 2번 > 3번
            // - 1번 ~ 2번, 2번 ~ 3번 스킬을 누를 수 있는 제한시간 있음, 제한시간을 테두리로 표현

            //if (_skill.Duration > 0f)
            // borderImage.fillAmount = 1f (_skill.CurrentDuration / _skill.Duration);

            await Awaitable.EndOfFrameAsync();

        }

        if (!Skill.IsInState<CooldownState>())
            slider.value = 0;
        
    }
}
