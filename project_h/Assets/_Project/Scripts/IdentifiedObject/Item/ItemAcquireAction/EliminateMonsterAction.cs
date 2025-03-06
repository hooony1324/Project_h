using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;



[System.Serializable]
public class EliminateMonstersAction : ItemAcquireAction
{
    // 현재 소환된 몬스터를 처치한다
    public override void AqcuireAction(Item owner) 
    {
        this.owner = owner;

        Debug.Log("EliminateMonstersAction");
    }
}
