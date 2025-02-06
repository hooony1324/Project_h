using Cysharp.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class SpawnNpcAction : DungeonSpawnAction
{
    [SerializeField] private Npc npcPrefab;

    private DungeonRoom owner;
    public override void Apply(DungeonRoom dungeonRoom)
    {
        this.owner = dungeonRoom;
        _ = SpawnNpc();
    }

    async UniTask SpawnNpc()
    {
        if (!string.IsNullOrEmpty(spawnEffect))
            Managers.Object.SpawnEffect(spawnEffect, spawnPoint.position, Quaternion.identity);

        await UniTask.WaitForSeconds(0.4f);

        GameObject.Instantiate(npcPrefab, owner.transform);
    }


}