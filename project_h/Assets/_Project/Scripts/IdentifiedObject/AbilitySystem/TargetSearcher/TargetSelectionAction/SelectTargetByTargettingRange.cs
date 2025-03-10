using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 추후 : defaultAttackRange보다 긴 사정거리의 스킬 용도 (ex.저격)
[System.Serializable]
public class SelectTargetByTargettingRange : TargetSelectionAction
{
    public StatScaleFloat targettingRange;

    [Header("Layer")]
    [SerializeField]
    private LayerMask layerMask;

    public override object Range => 0;
    public override object ScaledRange => 0;
    public override float Angle => 0;

    public SelectTargetByTargettingRange() { }
    public SelectTargetByTargettingRange(SelectTargetByTargettingRange copy)
        : base(copy)
    {
        layerMask = copy.layerMask;
        targettingRange = copy.targettingRange;
    }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        return new(requesterObject, SearchResultMessage.FindPosition);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        Entity target = null; 
        float closestDistance = float.MaxValue;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, targettingRange.GetValue(requesterEntity.StatsComponent), requesterEntity.EnemyLayerMask);
        foreach(Collider2D collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            if (entity == null)
                continue;

            float distance = Vector2.Distance(position, collider.bounds.center);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = entity;
            }
        }

        if (!target)
            return new TargetSelectionResult(position, SearchResultMessage.Fail);
        
        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindTarget);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        onSelectCompleted.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, requesterEntity.Position));
    }

    public override void CancelSelect(TargetSearcher targetSearcher)
    {
        
    }

    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        float distance = Vector2.Distance(requesterEntity.Position, targetPosition);
        float properRange = targettingRange.GetValue(requesterEntity.StatsComponent);
        return distance <= properRange;
    }
    public override object Clone() => new SelectTargetByTargettingRange(this);


}
