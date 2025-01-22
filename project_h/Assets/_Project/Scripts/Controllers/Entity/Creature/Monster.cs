using System.Collections;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using System.Threading;
using System;
using Unity.Behavior;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine;


public class Monster : Entity
{
    MonsterData monsterData;
    UI_WorldText infoText;
    BehaviorGraphAgent bga;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        layerMask = Util.GetLayerMask("Monster");
        enemyLayerMask = Util.GetLayerMask("Hero");

        bga = GetComponent<BehaviorGraphAgent>();

        return true;
    }
    
    public override void SetData(EntityData data)
    {
        base.SetData(data);

        bga.Restart();

        onDead += HandleOnDead;

        monsterData = data as MonsterData;
    }

    private void HandleOnDead(Entity entity)
    {
        Target = null;
        Movement.TraceTarget = null;

        _ = DropItem(monsterData.DropTableID);
        
        Invoke("Despawn", 3.0f);
    }

    private void Despawn()
    {
        cts_dropItem.Cancel();
        cts_dropItem.Dispose();

        Managers.Object.Despawn(this);
    }

    void Update()
    {
        if (StateMachine != null)
        {
            infoText.SetInfo(StateMachine.GetCurrentState().ToString());
        }
    }


    #region Item Drop

    private CancellationTokenSource cts_dropItem = new();
    private async UniTask DropItem(int dropTableID)
    {
        try
        {
            List<DropData> dropDatas = GetDropDatasByProbability(dropTableID);

            if (dropDatas == null || dropDatas.Count == 0)
                return;

            foreach (DropData dropData in dropDatas)
            {
                // 취소 요청 확인
                if (cts_dropItem.Token.IsCancellationRequested)
                    return;

                SpawnItemHolder(dropData);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cts_dropItem.Token);
            }
        }
        catch (OperationCanceledException)
        {
            // 취소 처리
            Debug.Log("아이템 드롭이 취소되었습니다.");
        }
    }

    /// <summary>
    /// probability 100인 아이템을 데이터의 앞 쪽에
    /// probability 100이 아닌 아이템들의 probability합이 100이 안되면 스폰 아이템 0개 상황 발생할 수도 있음
    /// </summary>
    private List<DropData> GetDropDatasByProbability(int dropTableID)
    {
        if (Managers.Data.DropTableDatas.TryGetValue(dropTableID, out List<DropData> dropDatas) == false)
            return null;

        if (dropDatas.Count == 0)
            return null;

        List<DropData> result = new();
        int sum = 0;
        int randomValue = Random.Range(0, 100);

        foreach (DropData dropData in dropDatas)
        {
            // item테이블 확인하여 더 이상 소환 할 수 없는 아이템인지 확인
            if (!IsDropEnable(dropData))
                continue;

            // 확정 드롭 아이템
            if (dropData.probability == 100)
            {
                result.Add(dropData);
                continue;
            }

            // 확정 드롭 제외한 나머지 아이템
            sum += dropData.probability;
            if (randomValue <= sum)
            {
                result.Add(dropData);
                break;
            }
        }

        return result;
    }

    private void SpawnItemHolder(DropData dropData)
    {
        ItemHolder itemHolder = Managers.Object.Spawn<ItemHolder>(CenterPosition);
        itemHolder.Setup(dropData);
    }

    // 아이템이 DefaultSkill인지 확인하고 소환할 수 있는지 확인
    private bool IsDropEnable(DropData dropData)
    {
        Item item = Managers.Data.GetItemData(dropData.itemID);
        bool isMultiple = Managers.Inventory.IsMultiple(item.ID);

        if ((item.IsAllowMultiple == false) && isMultiple)
            return false;
        
        if (item.IsSpawnable == false)
            return false;

        return true;
    }

    #if UNITY_EDITOR
    // Test
    public void TestDropItem(int dropTableID)
    {
        _ = DropItem(dropTableID);
    }
    #endif

    #endregion 
}