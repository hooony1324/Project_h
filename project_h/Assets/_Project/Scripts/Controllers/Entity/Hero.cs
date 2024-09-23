using UnityEngine;

public class Hero : Entity
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}