using UnityEngine;

public class Dummy : Entity
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        enemyLayer = Util.GetLayerMask("Hero");

        SetData(Managers.Data.GetMonsterData("MONSTER_DUMMY"));
        return true;
    }
}