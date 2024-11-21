using UnityEngine;

public class TestScene_ProceduralMap : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;




        return true;
    }

    async Awaitable LoadResources()
    {
        Managers.Resource.LoadAllAsync<Object>("PreTitle", (key, current, total) =>
        {
            if (current == total)
            {
                
            }
        });
    }

    public override void Clear()
    {
        
    }
}
