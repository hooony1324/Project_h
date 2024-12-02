using UnityEngine;

[System.Serializable]
public class SelectByMovingDirection : TargetSelectionAction
{
    public SelectByMovingDirection() { }
    public SelectByMovingDirection(SelectByMovingDirection copy)
        : base(copy)
    {
    }

    public override object Range => 0;

    public override object ScaledRange => 0;

    public override float Angle => 0;

    public override void CancelSelect(TargetSearcher targetSearcher)
    {
    }

    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        return true;
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        onSelectCompleted.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, requesterEntity.Movement.MovedDirection));
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 position)
    {
        return new TargetSelectionResult(requesterEntity.Movement.MovedPosition, SearchResultMessage.FindPosition);
    }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 position)
    {
        return SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject, position);
    }

    public override object Clone() => new SelectByMovingDirection(this);
}