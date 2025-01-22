using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 근처의 적을 자동으로 선택
// Entity IsInRange판단을 AttackRangeStat으로
[System.Serializable]
public class SelectPositionByAttackRange : TargetSelectionAction
{
    public override object Range => 0;
    public override object ScaledRange => 0f;
    public override float Angle => 0f;

    [SerializeField]
    private bool isSelectSameCategory;

    public SelectPositionByAttackRange() { }
    public SelectPositionByAttackRange(SelectPositionByAttackRange copy) : base(copy) { }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position) => SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, position);

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        var target = requesterEntity.Target;

        if (!target)
            return new TargetSelectionResult(position, SearchResultMessage.Fail);
        
        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindPosition);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        if (requesterEntity.Target == null)
        {
            onSelectCompleted.Invoke(new TargetSelectionResult(requesterObject, SearchResultMessage.Fail));
            return;
        }

        onSelectCompleted.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, requesterEntity.Target.Position));
    }

    public override void CancelSelect(TargetSearcher targetSearcher)
    {

    }

    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        float distance = Vector2.Distance(requesterEntity.Position, targetPosition);

        float attackRange = requesterEntity.StatsComponent.GetValue(requesterEntity.StatsComponent.AttackRangeStat);
        
        return !(distance > attackRange);
    }

    public override object Clone() => new SelectPositionByAttackRange(this);
}