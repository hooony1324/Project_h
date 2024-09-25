using UnityEngine;

public class Monster : Entity
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
    }
}