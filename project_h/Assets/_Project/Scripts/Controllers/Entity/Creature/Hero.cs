using System.Collections;
using NavMeshPlus.Extensions;
using UnityEngine;
using static Define;

public class Hero : Entity
{
    // Experimental Data -> TODO: Data Sheet
    string _animControllerName = "Hero_Warrior";

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
        
        Animator.runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>(_animControllerName);
        
    }


}