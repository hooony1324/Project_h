using System.Linq;
using UnityEngine;

/// <summary>
/// 근처의 적을 자동 선택하여 Target 선택
/// </summary>
[System.Serializable]
public class SelectNearestEntity : TargetSelectionAction
{
    public override object Range => 0f;
    public override object ScaledRange => 0f;
    public override float Angle => 0f;

    [SerializeField]
    private bool isSelectSameCategory;

    public SelectNearestEntity() { }
    public SelectNearestEntity(SelectNearestEntity copy) : base(copy) { }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        return SelectImmediate(targetSearcher, requesterEntity, requesterObject, Vector2.zero);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        return SelectImmediate(targetSearcher, requesterEntity, requesterObject, Vector2.zero);
    }

    private TargetSelectionResult SelectImmediate(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        float searchRange = requesterEntity.Stats.GetValue(requesterEntity.Stats.SearchRangeStat);
        var colliders = Physics2D.OverlapCircleAll(requesterEntity.Position, searchRange);

        float nearestDistance = Mathf.Infinity;
        Entity nearestEntity = null;
        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity == requesterEntity || entity.IsDead)
                continue;

            var hasCategory = requesterEntity.Categories.Any(x => entity.HasCategory(x));
            if ((hasCategory && !isSelectSameCategory) || (!hasCategory && isSelectSameCategory))
                continue;

            float distance = Vector2.Distance(entity.Position, position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEntity = entity;
            }
        }

        if (nearestEntity == null)
            return new TargetSelectionResult(requesterEntity.gameObject, SearchResultMessage.Fail);

        float attackRange = requesterEntity.Stats.GetValue(requesterEntity.Stats.AttackRangeStat);
        if (nearestDistance > attackRange)
            return new TargetSelectionResult(nearestEntity.gameObject, SearchResultMessage.OutOfRange);

        return new TargetSelectionResult(nearestEntity.gameObject, SearchResultMessage.FindTarget);
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        onSelectCompleted.Invoke(SelectImmediate(targetSearcher, requesterEntity, requesterObject, requesterEntity.Position));
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

    public override object Clone() => new SelectNearestEntity(this);
}