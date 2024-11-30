using UnityEngine;

[System.Serializable]
public class StraightMotion : ProjectileMotion
{
    public override void Setup(Projectile owner, Skill skill)
    {
        base.Setup(owner, skill);

        owner.Direction = skill.TargetSelectionResult.selectedPosition - Owner.transform.position;
    }

    public override void Move()
    {
        Owner.transform.position += Owner.Speed * Time.deltaTime * Owner.Direction;
    }
}