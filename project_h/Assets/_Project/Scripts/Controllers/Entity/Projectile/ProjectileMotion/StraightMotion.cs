using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable]
public class StraightMotion : ProjectileMotion
{
    public StraightMotion() {}
    public StraightMotion(StraightMotion other) : base(other) {}

    public override void Setup(Projectile owner, Skill skill)
    {
        base.Setup(owner, skill);
    }

    public override void Move()
    {
        Owner.transform.position += Owner.Direction * Owner.Speed * Time.deltaTime;
    }

    public override object Clone() => new StraightMotion(this);
}