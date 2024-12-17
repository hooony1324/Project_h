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
        // 반사
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_reflectCount > 0)
            {

                // 입사각과 반사각 계산
                Vector2 incomingVector = Owner.Direction;
                // contacts[0].normal -> contact가 여러개 발생 시 부정확함
                ContactPoint2D[] contacts = new ContactPoint2D[other.contactCount];
                other.GetContacts(contacts);

                Vector2 normal = Vector2.zero;
                for (int i = 0; i < contacts.Length; i++)
                {
                    normal += contacts[i].normal;
                }
                normal /= contacts.Length;
                
                Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

                // 반사 벡터를 방향으로 설정
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