using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 추후 : defaultAttackRange보다 긴 사정거리의 스킬 용도 (ex.저격)
[System.Serializable]
public class SelectAutoByTargettingRange : TargetSelectionAction
{
    [Min(0f)]
    [SerializeField]
    private float targettingRange;

    [Header("Layer")]
    [SerializeField]
    private LayerMask layerMask;

    public override object Range => targettingRange;
    public override object ScaledRange => 0;
    public override float Angle => 0;

    public SelectAutoByTargettingRange() { }
    public SelectAutoByTargettingRange(SelectAutoByTargettingRange copy)
        : base(copy)
    {
        layerMask = copy.layerMask;
    }
    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        return new(requesterObject, SearchResultMessage.FindPosition);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
    {
        var target = requesterEntity.Target;

        if (!target)
            return new TargetSelectionResult(position, SearchResultMessage.Fail);
        
        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindTarget);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        onSelectCompleted.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, requesterEntity.Target.Position));
    }

    public override void CancelSelect(TargetSearcher targetSearcher)
    {
        
    }

    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        float distance = Vector2.Distance(requesterEntity.Position, targetPosition);
        float properRange = (float)ProperRange;
        
        return !(distance > properRange);
    }
    public override object Clone() => new SelectAutoByTargettingRange(this);


}
