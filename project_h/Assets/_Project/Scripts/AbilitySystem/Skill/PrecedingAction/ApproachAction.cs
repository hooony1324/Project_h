using UnityEngine;


// Player Select 방식은 Reserved Skill에 의해 접근하지만
// AI 는 Reserved Skill방식으로 진행하면 복잡(ex. ChargeState전에 접근하는 경우)
// 따라서 별도의 접근 액션을 만들어줌
[System.Serializable]
public class ApproachAction : SkillPrecedingAction
{
    // 적 탐색 범위
    [SerializeField]
    private float searchRange;

    // 접근 거리
    [SerializeField]
    private float remainingDistance;

    
    private Entity target = null;

    public override void Start(Skill skill)
    {
        var collider = Physics2D.OverlapCircle(skill.Owner.Position, searchRange, skill.Owner.EnemyLayerMask);
        if (collider == null)
            return;
        
        target = collider.GetComponent<Entity>();
    }

    public override bool Run(Skill skill)
    {
        if (target == null)
            return true;
        
        float distance = Vector2.Distance(skill.Owner.Position, target.Position);
        return distance < remainingDistance;
    }

    public override object Clone() => new ApproachAction();
}