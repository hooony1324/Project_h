using UnityEngine;

[System.Serializable]
public class SpawnProjectileAction : SkillAction
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private string spawnPointSocketName;
    [SerializeField]
    private float speed;

    [SerializeReference, SubclassSelector]
    private ImpactAction impactAction;

    [SerializeReference, SubclassSelector]
    private ProjectileMotion projectileMotion;
    
    public override void Apply(Skill skill)
    {
        var socket = skill.Owner.GetFireSocket();
        var projectile = Managers.Object.SpawnProjectile(projectilePrefab, socket.position, socket.rotation);
        projectile.transform.position = socket.position;

        projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, socket.forward, skill, impactAction, projectileMotion);
    }

    public override object Clone()
    {
        return new SpawnProjectileAction()
        {
            projectilePrefab = projectilePrefab,
            spawnPointSocketName = spawnPointSocketName,
            speed = speed
        };
    }
}
