using UnityEngine;

[System.Serializable]
public class ImpactAndDestroy : ImpactAction
{
    public ImpactAndDestroy() {}
    public ImpactAndDestroy(ImpactAndDestroy other) : base(other) {}

    public override void Apply(Collision2D other)
    {
        Entity target = other.collider.GetComponent<Entity>();
        if (target == null)
            return;

        target.SkillSystem.Apply(Skill);

        if (ImpactEffect != null)
            Managers.Object.SpawnEffect(ImpactEffect, Owner.transform.position, Quaternion.identity);

        Managers.Object.DespawnProjectile(Owner);
    }

    public override object Clone() => new ImpactAndDestroy(this);
}