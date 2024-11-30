using UnityEngine;

[System.Serializable]
public abstract class ProjectileMotion
{
    private Projectile _owner;
    private Skill _skill;

    protected Projectile Owner => _owner;
    protected Skill Skill => _skill;

    public virtual void Setup(Projectile owner, Skill skill)
    {
        _owner = owner;
        _skill = skill;
    }

    public abstract void Move();
}