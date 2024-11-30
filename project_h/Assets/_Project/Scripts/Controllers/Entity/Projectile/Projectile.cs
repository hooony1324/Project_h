using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
    private Entity _owner;
    private float _speed;
    private Vector3 _direction;
    private Skill _skill;

    public Entity Owner => _owner;
    public Skill Skill => _skill;
    public float Speed => _speed;

    public Vector3 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            
            // direction 방향으로 회전
            float angle = Vector2.SignedAngle(Vector2.up, value);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    // Impact때 옵션 : Destroy, 튕김 등
    private ImpactAction _impactAction;

    // 움직임 옵션
    private ProjectileMotion _projectileMotion;

    private CountdownTimer _timer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Projectile;
        GetComponent<SpriteRenderer>().sortingOrder = SortingLayers.PROJECTILE;

        return true;
    }

    public void Setup(Entity owner, float speed, Vector3 direction, Skill skill, ImpactAction impactAction, ProjectileMotion projectileMotion)
    {
        _owner = owner;
        _speed = speed;
        _skill = skill;
        _direction = direction;

        _impactAction = impactAction;
        _projectileMotion = projectileMotion;

        _impactAction.Setup(this, skill);
        _projectileMotion.Setup(this, skill);

        _timer = new CountdownTimer(_skill.Duration);
        _timer.OnTimerStop += () =>
        {
            Managers.Object.DespawnProjectile(this);
        };

        _timer.Start();
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
        _projectileMotion.Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity target = other.GetComponent<Entity>();

        if (target == _owner)
            return;

        // 자신이 아니면 무조건 Apply
        if (target)
        {
            target.SkillSystem.Apply(_skill);
            _impactAction.Apply();
        }

    }
}