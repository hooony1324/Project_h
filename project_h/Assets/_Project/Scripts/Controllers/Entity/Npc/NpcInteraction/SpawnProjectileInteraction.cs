using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

[Serializable]
public class SpawnProjectileInteraction : NpcInteraction
{    
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float lifetime;

    [SerializeReference, SubclassSelector]
    private ImpactAction impactAction;

    [SerializeReference, SubclassSelector]
    private ProjectileMotion projectileMotion;


    private Npc _owner;

    public override void Setup(Npc npc)
    {
        _owner = npc;
    }

    public override void HandleNpcInteraction()
    {
        if (_owner == null)
            return;

        GameObject projectile = GameObject.Instantiate(projectilePrefab, _owner.CenterPosition, Quaternion.identity);

        projectile.GetComponent<Projectile>().Setup(null, speed, direction.normalized, null, lifetime, impactAction, projectileMotion);
    }
}