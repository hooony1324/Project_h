using UnityEngine;

[System.Serializable]
public class StraightMotion : ProjectileMotion
{
    public StraightMotion() {}
    public StraightMotion(StraightMotion other) : base(other) {}

    public override void Setup(Projectile owner, Skill skill, Vector3 direction)
    {
        base.Setup(owner, skill, direction);

        float angle = Vector2.SignedAngle(Vector2.up, Direction);
        Owner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public override void Move()
    {
        Owner.transform.Translate(Owner.Speed * Time.deltaTime * Vector3.up);
    }

    public override object Clone() => new StraightMotion(this);
}