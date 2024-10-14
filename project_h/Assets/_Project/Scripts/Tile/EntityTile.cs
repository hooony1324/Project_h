using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityTile : Tile
{
    // 스폰할 몬스터 Addressable Name
    [SerializeField]
    private string prefabName;

    // 스폰할 몬스터의 Data
    public EntityData entityData;
}