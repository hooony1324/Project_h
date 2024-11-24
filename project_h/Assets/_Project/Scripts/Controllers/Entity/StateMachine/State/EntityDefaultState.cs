using static Define;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

/// <summary> 주변에 적을 감지하면 추적하여 공격한다 </summary>
public class EntityDefaultState : State<Entity>
{


    protected override void Setup() 
    {

    }

    public override void Enter() 
    {
        
    }

    public override void Update() 
    {
        
    }

    public override void Exit() 
    {
        
    }


    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill skill, AnimatorParameter animatorParameter))data;
        Entity.Animator?.SetTrigger(tupleData.Item2.Hash);

        return true;
    }
}