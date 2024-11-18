using System.Collections;
using JetBrains.Annotations;
using NavMeshPlus.Extensions;
using UnityEngine;
using static Define;

public class Hero : Entity
{
    public override bool IsMoving => IsMainHero ? Movement.IsForcedMoving : base.IsMoving;
    public bool IsMainHero { get; set; }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;


        return true;
    }

    public override void SetData(EntityData data)
    {
        base.SetData(data);
        
        
    }
}