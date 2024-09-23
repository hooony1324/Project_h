using UnityEngine;

public class Entity : InitOnce
{
    public Vector3 Position => transform.position;
    public Vector3 CenterPosition => transform.position + CenterOffset;
    public Vector3 CenterOffset {get; set;} = new Vector3(0, 0.85f, 0);

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }


}