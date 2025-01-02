using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Threading;
using System;
using R3;
using Unity.Behavior;


public class Monster : Entity
{

    UI_WorldText infoText;
    BehaviorGraphAgent bga;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        layerMask = Util.GetLayerMask("Monster");
        enemyLayerMask = Util.GetLayerMask("Hero");

        bga = GetComponent<BehaviorGraphAgent>();

        return true;
    }
    
    public override void SetData(EntityData data)
    {
        base.SetData(data);

        bga.Restart();

        onDead += HandleOnDead;
    }

    private void HandleOnDead(Entity entity)
    {
        Target = null;
        Movement.TraceTarget = null;
        
        Invoke("Despawn", 3.0f);
    }

    private void Despawn()
    {
        Managers.Object.Despawn(this);
    }

    void Update()
    {
        if (StateMachine != null)
        {
            infoText.SetInfo(StateMachine.GetCurrentState().ToString());
        }
    }

}