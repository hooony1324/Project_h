using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonManager
{
    public Dungeon CurrentDungeon { get; private set; }

    public DungeonData CurrentDungeonData => _currentDungeonData;
    private DungeonData _currentDungeonData;
    public int CurrentDungeonId => _currentDungeonData ? _currentDungeonData.Id : 0;

    // 몬스터 처치1 > 몬스터 처치2 상황 드랍 아이템 겹치는 경우 안생기도록 스폰한 아이템 캐시
    public HashSet<int> SpawnedEquipmentItemsCache = new();


    public DungeonRoom CurrentRoom;

    public void Setup(Dungeon dungeon)
    {
        CurrentDungeon = dungeon;
    }

    public void SetFirstDungeon(DungeonData firstDungeonData)
    {
        _currentDungeonData = firstDungeonData;
    }

    public void EnterFirstDungeon()
    {
        if (_currentDungeonData == null)
            return;

        Managers.Map.SetMap(_currentDungeonData.PrefabName);
        Managers.Scene.LoadScene(EScene.DungeonScene);

        
    }

    public void TryEnterNextDungeon()
    {
        if (_currentDungeonData == null)
            return;

        if (!_currentDungeonData.HasNextDungeon)
        {
            // 마지막 던전 -> 마을
            if  (_currentDungeonData.IsFinalDungeon)
            {
                
                Managers.Map.SetMap("BaseMap");
                Managers.Scene.LoadScene(EScene.GameScene);
            }

            return;
        }

        // 스탯 저장 -> 다음 던전 시작 시 적용
        //_heroStats = Managers.Hero.MainHero.Stats;
        
        DungeonData nextDungeonData = Managers.Data.GetDungeonData(_currentDungeonData.NextDungeonId);
        if (nextDungeonData == null)
        {
            Debug.Log("다음 던전 데이터를 설정하지 않았습니다.");
            return;
        }
        
        // 재입장 할 경우의 상황 저장
        _currentDungeonData = nextDungeonData;
        // HeroInfo = asdfasdfsda;

        // 다음 던전으로 입장
        Managers.Map.SetMap(nextDungeonData.PrefabName);
        Managers.Scene.LoadScene(EScene.DungeonScene);

        return;
    }

    public void Clear()
    {
        CurrentDungeon = null;
        _currentDungeonData = null;

        cts_dropItem.Cancel();
        cts_dropItem.Dispose();
        cts_dropItem = null;

        SpawnedEquipmentItemsCache.Clear();
    }

    public async Awaitable GenerateDungeon()
    {
        await CurrentDungeon?.GenerateDungeon();
    }


    #region Item Drop

    private CancellationTokenSource cts_dropItem = new();
    public async UniTask DropItem(int dropGroupID, Vector3 position)
    {
        cts_dropItem?.Cancel();
        cts_dropItem?.Dispose();

        cts_dropItem = new CancellationTokenSource();

        try
        {
            List<int> dropItems = GetDropItemsByProbability(dropGroupID);

            if (dropItems == null || dropItems.Count == 0)
                return;

            foreach (int itemID in dropItems)
            {
                SpawnItemHolder(itemID, position);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cts_dropItem.Token);
            }
        }
        catch (OperationCanceledException exception)
        {
            Debug.LogError(exception);
        }
    }

    /// <summary>
    /// probability 100인 아이템을 데이터의 앞 쪽에
    /// probability 100이 아닌 아이템들의 probability합이 100이 안되면 스폰 아이템 0개 상황 발생할 수도 있음
    /// </summary>
    private List<int/*itemId*/> GetDropItemsByProbability(int dropGroupID)
    {
        List<DropTableData> itemGroup = Managers.Data.DropTableDatas.Where(x => x.GroupID == dropGroupID).ToList();

        if (itemGroup.Count == 0)
            return null;

        List<int> result = new();
        float sum = 0;
        float randomValue = Random.Range(0, 1000) / 10f; // 0.1 단위로 생성 (0.0, 0.1, 0.2, ..., 99.9)

        foreach (DropTableData dropData in itemGroup)
        {
            // item테이블 확인하여 더 이상 소환 할 수 없는 아이템인지 확인
            if (!IsDropEnable(dropData.ItemID))
                continue;

            // 확정 드롭 아이템
            if (Mathf.Approximately(dropData.Probability, 100f))
            {
                result.Add(dropData.ItemID);
                continue;
            }

            // 확정 드롭 제외한 나머지 아이템
            sum += dropData.Probability;
            if (randomValue <= sum)
            {
                result.Add(dropData.ItemID);
                break;
            }
        }

        return result;
    }

    private void SpawnItemHolder(int itemID, Vector3 position)
    {
        ItemHolder itemHolder = Managers.Object.Spawn<ItemHolder>(position);
        itemHolder.Setup(itemID);
    }

    // 아이템이 DefaultSkill인지 확인하고 소환할 수 있는지 확인
    private bool IsDropEnable(int itemID)
    {
        Item item = Managers.Data.GetItemData(itemID);
        bool isMultiple = Managers.Inventory.IsMultiple(item.ID);

        bool isSpawnedEquipment = SpawnedEquipmentItemsCache.Contains(itemID);
        if (isSpawnedEquipment)
            return false;

        if ((item.IsAllowMultiple == false) && isMultiple)
            return false;
        
        if (item.IsSpawnable == false)
            return false;

        if (item.IsEquipment)
            SpawnedEquipmentItemsCache.Add(item.ID);
        return true;
    }

    #if UNITY_EDITOR
    // Test
    public void TestDropItem(int dropTableID, Vector3 position)
    {
        _ = DropItem(dropTableID, position);
    }
    #endif

    #endregion 
}