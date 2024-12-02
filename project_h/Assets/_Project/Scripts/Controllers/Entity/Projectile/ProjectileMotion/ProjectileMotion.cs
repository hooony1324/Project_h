using System;
using UnityEngine;

[System.Serializable]
public abstract class ProjectileMotion : ICloneable
{
    private Vector3 _direction;
    private Projectile _owner;
    private Skill _skill;

    protected Projectile Owner => _owner;
    protected Skill Skill => _skill;
    protected Vector3 Direction => _direction;

    public ProjectileMotion() {}
    public ProjectileMotion(ProjectileMotion other)
    {
        _direction = other._direction;
        _owner = other._owner;
        _skill = other._skill;
    }

    public virtual void Setup(Projectile owner, Skill skill, Vector3 direction)
    {
        _owner = owner;
        _skill = skill;
        _direction = direction;
    }

    public abstract void Move();

    public abstract object Clone();
}
