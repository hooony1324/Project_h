using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestScene_ProceduralMap : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;




        return true;
    }

    private async UniTask LoadAddressableAssets()
    {
        bool bResourceLoaded = false;

        Managers.Resource.LoadAllAsync<Object>("PreGame", (key, current, total) =>
        {
            if (current == total)
            {
                bResourceLoaded = true;
            }
        });

        await UniTask.WaitUntil(() => bResourceLoaded);
    }

    async void Start()
    {
        await LoadAddressableAssets();
    }

    public override void Clear()
    {
        
    }
}
