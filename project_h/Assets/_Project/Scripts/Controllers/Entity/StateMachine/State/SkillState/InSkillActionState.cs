public class InSkillActionState : EntitySkillState
{
    public bool IsStateEnded { get; private set; }

    public override void Update()
    {
        // AnimatorParameter가 false라면 State를 종료
        if (RunningSkill.InSkillActionFinishOption == InSkillActionFinishOption.FinishWhenAnimationEnded)
            IsStateEnded = !Entity.Animator.GetBool(AnimatorParameterHash);
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        // 올바른 Message가 아니라면 false를 return
        if (!base.OnReceiveMessage(message, data))
            return false;

        Entity.Movement.CanRotate = false;  // 회전 비활성화

        if (RunningSkill.InSkillActionFinishOption != InSkillActionFinishOption.FinishWhenAnimationEnded)
            RunningSkill.onApplied += OnSkillApplied;

        return true;
    }
    
    public override void Exit()
    {
        IsStateEnded = false;
        RunningSkill.onApplied -= OnSkillApplied;

        Entity.Movement.CanRotate = true;   // 회전 다시 활성화
        Entity.Movement.TraceTarget = null;

        base.Exit();
    }

    private void OnSkillApplied(Skill skill, int currentApplyCount)
    {
        switch (skill.InSkillActionFinishOption)
        {
            // Skill이 한번이라도 적용되었다면 State를 종료
            case InSkillActionFinishOption.FinishOnceApplied:
                IsStateEnded = true;
                break;

            // Skill이 모두 적용되었다면 State를 종료
            case InSkillActionFinishOption.FinishWhenFullyApplied:
                IsStateEnded = skill.IsFinished;
                break;
        }
    }
}
