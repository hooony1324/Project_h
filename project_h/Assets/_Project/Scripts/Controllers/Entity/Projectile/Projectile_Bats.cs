using UnityEngine;

public class Projectile_Bats : Projectile
{
    // BatsController에 Skill과 
    BatsController batsController;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        batsController = Util.FindChild(gameObject, "BatsController").GetComponent<BatsController>();

        return true;
    }

    public override void Setup(Entity owner, float speed, Vector3 direction, Skill skill, float lifetime, ImpactAction impactAction, ProjectileMotion projectileMotion)
    {
        _owner = owner;
        _speed = speed;
        _skill = skill;
        _direction = direction;

        transform.position = Managers.Dungeon.CurrentRoom.TeleportPoint.position;
        transform.SetParent(Managers.Dungeon.CurrentRoom.TeleportPoint);

        batsController.Setup(this, skill);
    }

    protected override void UpdateMovement() { }
}