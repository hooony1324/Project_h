using System.Collections;
using JetBrains.Annotations;
using NavMeshPlus.Extensions;
using UnityEngine;
using static Define;

public class Hero : Entity
{
    public override bool IsMoving => IsMainHero ? Movement.IsForcedMoving : base.IsMoving;
    public bool IsMainHero { get; set; }

    UI_WorldText infoText;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;
        infoText = Util.FindChild<UI_WorldText>(gameObject);

        return true;
    }

    public override void SetData(EntityData data)
    {
        base.SetData(data);
        
        
    }

    void Update()
    {
        infoText.SetInfo(StateMachine.GetCurrentState().ToString());
    }

    public override void FindNearestEnemy()
    {
        if (Target != null)
            return;

        float searchRange = Stats.GetValue(Stats.SearchRangeStat);

        int monsterLayer = Util.GetLayerMask("Monster");
        var colliders = Physics2D.OverlapCircleAll(Position, searchRange, monsterLayer);

        float nearestDistance = Mathf.Infinity;
        Entity nearestEnemy = null;
        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity == this || entity.IsDead)
                continue;

            if (!entity.HasCategory(enemyCategory))
                continue;

            float distance = Vector2.Distance(entity.Position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = entity;
            }
        }

        if (nearestEnemy)
        {
            Target = nearestEnemy;
            return;
        }

        Target = null;
        return;
    }

}