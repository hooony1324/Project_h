using UnityEngine;

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
        // 반사
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_reflectCount > 0)
            {
                // 입사각과 반사각 계산
                Vector2 normal = other.contacts[0].normal; 
                Vector2 incomingVector = Owner.Direction;
                
                Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

                // 반사 벡터를 방향으로 설정
                Owner.Direction = reflectVector;

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
            Entity target = other.collider.GetComponent<Entity>( );
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