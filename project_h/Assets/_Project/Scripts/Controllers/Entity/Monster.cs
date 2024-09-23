using UnityEngine;

public class Monster : Entity
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}