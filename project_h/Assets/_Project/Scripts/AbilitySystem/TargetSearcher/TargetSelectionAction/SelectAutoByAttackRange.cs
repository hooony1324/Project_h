using System.Linq;
using UnityEngine;

// 근처의 적을 자동으로 선택
// Entity IsInRange판단을 AttackRangeStat으로
[System.Serializable]
public class SelectAutoByAttackRange : TargetSelectionAction
{
    public override object Range => 0;
    public override object ScaledRange => 0f;
    public override float Angle => 0f;

    [SerializeField]
    private bool isSelectSameCategory;

    public SelectAutoByAttackRange() { }
    public SelectAutoByAttackRange(SelectAutoByAttackRange copy) : base(copy) { }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position) => SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, position);

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        var target = requesterEntity.Target;

        // TODO: selectSameCategory 추가

        if (!target)
            return new TargetSelectionResult(position, SearchResultMessage.Fail);
        
        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindTarget);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    // private TargetSelectionResult SelectImmediate(TargetSearcher targetSearcher, Entity requesterEntity,
    //     GameObject requesterObject, Vector3 position)
    // {
    //     Entity nearestEntity = null;

    //     var target = requesterEntity.Target;

    //     if (!target)
    //         return new TargetSelectionResult(requesterEntity.gameObject, SearchResultMessage.Fail);

    //     float attackRange = requesterEntity.Stats.GetValue(requesterEntity.Stats.AttackRangeStat);
    //     float nearestDistance = Vector2.Distance(target.Position, position);

    //     // OutOfRange > 당장 사용 못함 > ReservedSkill에 등록됨
    //     if (nearestDistance > attackRange)
    //         return new TargetSelectionResult(nearestEntity.gameObject, SearchResultMessage.OutOfRange);

    //     return new TargetSelectionResult(nearestEntity.gameObject, SearchResultMessage.FindTarget);
    // }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        if (requesterEntity.Target == null)
        {
            //onSelectCompleted.Invoke(SelectImmediate(targetSearcher, requesterEntity, requesterObject, requesterEntity.Position));
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

        float attackRange = requesterEntity.Stats.GetValue(requesterEntity.Stats.AttackRangeStat);
        
        return !(distance > attackRange);
    }

    public override object Clone() => new SelectAutoByAttackRange(this);
}