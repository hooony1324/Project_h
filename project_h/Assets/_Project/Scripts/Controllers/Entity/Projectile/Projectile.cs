using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
    protected Entity _owner;
    protected float _speed;
    protected Skill _skill;
    private Rigidbody2D _rigidbody;

    public Entity Owner => _owner;
    public Skill Skill => _skill;
    public float Speed => _speed;
    public Rigidbody2D Rigidbody => _rigidbody;

    // Impact때 옵션 : Destroy, 튕김 등
    private ImpactAction _impactAction;
    // 움직임 옵션
    private ProjectileMotion _projectileMotion;

    private CancellationTokenSource _destroyCTS;

    protected Vector3 _direction;
    public Vector3 Direction 
    {
        get => _direction;
        set 
        {
            _direction = value;
            
            float angle = Vector2.SignedAngle(Vector2.up, value);
            transform.rotation = Quaternion.identity;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Projectile;

        return true;
    }

    public virtual void Setup(Entity owner, float speed, Vector3 direction, Skill skill, float lifetime, ImpactAction impactAction, ProjectileMotion projectileMotion)
    {
        _owner = owner;
        _speed = speed;
        _skill = skill;
        Direction = direction;

        GetComponent<SpriteRenderer>().sortingOrder = SortingLayers.PROJECTILE;
        _rigidbody = GetComponent<Rigidbody2D>();

        _impactAction = impactAction.Clone() as ImpactAction;
        _projectileMotion = projectileMotion.Clone() as ProjectileMotion;

        _impactAction.Setup(this, skill, impactAction.ImpactEffect);
        _projectileMotion.Setup(this, skill);

        SetLifeTime(lifetime).Forget();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    protected virtual void UpdateMovement()
    {
        _projectileMotion.Move();
    }

    private async UniTask SetLifeTime(float duration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _destroyCTS.Token);
        if (this != null)
            Managers.Object.DespawnProjectile(this);   
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Owner.gameObject)
            return;

        _impactAction.Apply(other);
    }

    private void OnEnable()
    {
        _destroyCTS = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        _destroyCTS?.Cancel();
        _destroyCTS?.Dispose();
    }
}