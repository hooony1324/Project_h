using System;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileMotion : ICloneable
{
    private Projectile _owner;
    private Skill _skill;

    protected Projectile Owner => _owner;
    protected Skill Skill => _skill;

    public ProjectileMotion() {}
    public ProjectileMotion(ProjectileMotion other)
    {
        _owner = other._owner;
        _skill = other._skill;
    }

    public virtual void Setup(Projectile owner, Skill skill)
    {
        _owner = owner;
        _skill = skill;
    }

    public abstract void Move();

    public abstract object Clone();
}
