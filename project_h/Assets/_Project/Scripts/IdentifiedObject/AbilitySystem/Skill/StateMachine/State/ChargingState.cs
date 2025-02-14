using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingState : SkillState
{
    // Charge 상태가 종료되었는가? true라면 다른 State로 전이됨
    public bool IsChargeEnded { get; private set; }
    // Charge가 최소 충전량을 채웠고, Skill이 기준점 검색에 성공했는가?(=Charge를 마치고 Skill이 사용되었는가?)
    // 위와 마찬가지로 true라면 다른 State로 전이됨
    public bool IsChargeSuccessed { get; private set; }


    public override void Enter()
    {
        Entity.Activate();

        Entity.Owner.Movement.enabled = false;

        if (Entity.Owner.IsPlayer)
        {
            Entity.SelectTarget(OnTargetSearchCompleted, false);
        }
        else
        {
            // 현재 상황 : SelectAction의 IsRange체크 실패 시 Charging Skill은 쓰지 못함
            // - Charge > Searching > InActionState로 전이, Searching에서 Reserve가능
            // => Charging Skill은 Reserve못함
            TargetSelectionResult result = Entity.SelectTargetImmediate(Entity.Owner.Position);
            if (result.resultMessage == SearchResultMessage.Fail || result.resultMessage == SearchResultMessage.OutOfRange)
            {
                Entity.Cancel();
                Entity.Owner.StateMachine.ExecuteCommand(EntityStateCommand.ToDefaultState);
                return;
            }
        }
        Entity.ShowIndicator();
        Entity.StartCustomActions(SkillCustomActionType.Charge);

        TrySendCommandToOwner(Entity, EntityStateCommand.ToChargingSkillState, Entity.ChargeAnimationParameter);
    }

    public override void Update()
    {
        Entity.CurrentChargeDuration += Time.deltaTime;

        if (!Entity.Owner.IsPlayer && Entity.IsMaxChargeCompleted)
        {
            IsChargeEnded = true;
            //Entity.SelectTarget(false);
            Entity.HideIndicator();
            TryUse();
        }
        else if (Entity.IsChargeDurationEnded)
        {
            IsChargeEnded = true;
            if (Entity.ChargeFinishActionOption == SkillChargeFinishActionOption.Use)
            {
                // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
                //     Entity.SelectTargetImmediate(hitInfo.point);

                // 어떤 동작이 있을 때 (ex) PointerUp
                //Entity.SelectTargetImmediate(Entity.Owner.Position);

                TryUse();
            }
        }

        Entity.RunCustomActions(SkillCustomActionType.Charge);
    }

    public override void Exit()
    {
        Entity.Owner.Movement.enabled = true;
        IsChargeEnded = false;
        IsChargeSuccessed = false;

        if (Entity.IsSearchingTarget)
            Entity.CancelSelectTarget();
        else
            Entity.HideIndicator();

        Entity.ReleaseCustomActions(SkillCustomActionType.Charge);
    }

    private bool TryUse()
    {
        if (Entity.IsMinChargeCompleted && Entity.IsTargetSelectSuccessful)
            IsChargeSuccessed = true;

        return IsChargeSuccessed;
    }

    private void OnTargetSearchCompleted(Skill skill, TargetSearcher searcher, TargetSelectionResult result)
    {
        if (!TryUse())
            Entity.SelectTarget(OnTargetSearchCompleted, false);
    }
}
