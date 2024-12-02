using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
    private Entity _owner;
    private float _speed;
    private Skill _skill;

    public Entity Owner => _owner;
    public Skill Skill => _skill;
    public float Speed => _speed;

    // Impact때 옵션 : Destroy, 튕김 등
    private ImpactAction _impactAction;

    // 움직임 옵션
    private ProjectileMotion _projectileMotion;

    private CancellationTokenSource _destroyCTS;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Projectile;
        GetComponent<SpriteRenderer>().sortingOrder = SortingLayers.PROJECTILE;

        return true;
    }

    public async UniTask Setup(Entity owner, float speed, Vector3 direction, Skill skill, ImpactAction impactAction, ProjectileMotion projectileMotion)
    {
        _owner = owner;
        _speed = speed;
        _skill = skill;

        _impactAction = impactAction.Clone() as ImpactAction;
        _projectileMotion = projectileMotion.Clone() as ProjectileMotion;

        _impactAction.Setup(this, skill, impactAction.ImpactEffect);
        _projectileMotion.Setup(this, skill, direction);

        if (_skill.Duration > 0)
        {
            _destroyCTS = new CancellationTokenSource();

            await UniTask.Delay(TimeSpan.FromSeconds(_skill.Duration), cancellationToken: _destroyCTS.Token);
            if (this != null)
                Managers.Object.DespawnProjectile(this);
        }
    }

    private void Update()
    {
        _projectileMotion.Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity target = other.GetComponent<Entity>();

        if (target == _owner || target == null)
            return;

        target.SkillSystem.Apply(_skill);
        _impactAction.Apply();
    }

    private void OnDisable()
    {
        _destroyCTS?.Cancel();
        _destroyCTS?.Dispose();
    }
}