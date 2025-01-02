using UnityEngine;
using static Define;

[System.Serializable]
public class ImpactAndReflect : ImpactAction
{
    [SerializeField]
    private int _reflectCount;

    
    public ImpactAndReflect() {}
    public ImpactAndReflect(ImpactAndReflect other) : base(other) 
    {
        _reflectCount = other._reflectCount;
    }

    public override void Apply(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_reflectCount > 0)
            {
                Vector2 incomingVector = Owner.Direction;
                Vector2 normal = other.contacts[0].normal;

                

                // 정확한 normal을 사용하여 반사 계산
                Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);
                
                Owner.Direction = reflectVector.normalized;
                _reflectCount--;
            }
            else
            {
                // if (ImpactEffect != null)
                //     Managers.Object.SpawnEffect(ImpactEffect, Owner.transform.position, Quaternion.identity);

                Managers.Object.DespawnProjectile(Owner);
            }
        }
        else
        {
            Entity target = other.collider.GetComponent<Entity>();
            if (target == null)
                return;

            target.SkillSystem.Apply(Skill);

            // if (ImpactEffect != null)
            //     Managers.Object.SpawnEffect(ImpactEffect, Owner.transform.position, Quaternion.identity);

            Managers.Object.DespawnProjectile(Owner);
        }

    }

    public override object Clone() => new ImpactAndReflect(this);
}