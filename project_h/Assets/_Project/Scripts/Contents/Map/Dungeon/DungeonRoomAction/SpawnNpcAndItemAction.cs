using Cysharp.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class SpawnNpcAndItemAction : DungeonSpawnAction
{
    [SerializeField] private Npc npcPrefab;
    [SerializeField] private int dropGroupID;

    private DungeonRoom owner;
    public override void Apply(DungeonRoom dungeonRoom)
    {
        this.owner = dungeonRoom;
        _ = SpawnNpc();
        _ = SpawnItem();
    }

    async UniTask SpawnNpc()
    {
        if (!string.IsNullOrEmpty(spawnEffect))
            Managers.Object.SpawnEffect(spawnEffect, spawnPoint.position, Quaternion.identity);

        await UniTask.WaitForSeconds(0.4f);

        GameObject.Instantiate(npcPrefab, owner.transform);
    }

    async UniTask SpawnItem()
    {
        Vector3 itemPosition = spawnPoint.position + new Vector3(0, -10, 0);;

        await UniTask.WaitForSeconds(1f);

        if (!string.IsNullOrEmpty(spawnEffect))
            Managers.Object.SpawnEffect(spawnEffect, itemPosition, Quaternion.identity);

        await UniTask.WaitForSeconds(0.4f);

        _ = Managers.Dungeon.DropItem(dropGroupID, itemPosition);
    }
}