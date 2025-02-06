using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SpawnItemAction : DungeonSpawnAction
{
    [SerializeField] private int dropGroupID;

    public override void Apply(DungeonRoom dungeonRoom)
    {
        _ = SpawnItem();
    }

    async UniTask SpawnItem()
    {
        await UniTask.WaitForSeconds(1f);

        if (!string.IsNullOrEmpty(spawnEffect))
            Managers.Object.SpawnEffect(spawnEffect, spawnPoint.position, Quaternion.identity);

        await UniTask.WaitForSeconds(0.4f);

        _ = Managers.Dungeon.DropItem(dropGroupID, spawnPoint.position);
    }
}