using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ITEM", menuName = "Item/AbilityItem")]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Sprite itemHolderSprite;
    [SerializeField, Tooltip("여러 개 획득할 수 있는지의 여부")] private bool isAllowMultiple = false;
    [SerializeField, Tooltip("장비 창에 보여줄지의 여부")] private bool isEquipment = true;
    [SerializeField, Tooltip("조합된 아이템 여부")] private bool isCombinedItem = false;



    [SerializeReference, SubclassSelector] public ItemAcquireAction[] itemAcquireActions;

    [SerializeField] private string description;

    public int ID => id;
    public Sprite ItemHolderSprite => itemHolderSprite;
    public bool IsAllowMultiple => isAllowMultiple;
    public bool IsEquipment => isEquipment;
    public bool IsCombinedItem => isCombinedItem;
    public bool IsSpawnable
    {
        get
        {
            if (itemAcquireActions.Length == 0)
                return false;

            return itemAcquireActions.All(x => x.IsSpawnable);
        }
    }

    public void Acquire(bool bLoadSaveData = false)
    {
        // 플레이 중 획득 처리
        if (!bLoadSaveData)
        {
            if (IsEquipment)
                Managers.Inventory.AddItem(this);

            foreach (var itemAcquireAction in itemAcquireActions)
            {
                itemAcquireAction.AqcuireAction(this);
            }
        }
        // 데이터 로드 시, 획득내용 복구
        else
        {
            if (IsEquipment)
                Managers.Inventory.AddEquippedItem(this);

            // stat 향상 Aqcuire는 Dynamic제외
            foreach (var itemAcquireAction in itemAcquireActions)
            {
                // 플레이 중 증감 수치(DynamicValue)가 있는 IncreaseStat은 복구 될 때 제외
                if (itemAcquireAction.IsSubclassOf<IncreaseStatItemAction>())
                {
                    IncreaseStatItemAction increaseStatItem = itemAcquireAction as IncreaseStatItemAction;
                    increaseStatItem.LoadPresistantStat();
                }
                else
                {
                    itemAcquireAction.AqcuireAction(this);
                }
            }
        }

    }

    public void Release()
    {
        foreach (var itemAcquireAction in itemAcquireActions)
        {
            itemAcquireAction.Release();
        }
    }

    public void LoadPresistantStats()
    {
        foreach (var itemAcquireAction in itemAcquireActions)
        {
            // IncreaseStatItem은 DynamicValue있어서 선택적으로 복구함
            if (!itemAcquireAction.IsSubclassOf<IncreaseStatItemAction>())
                continue;

            IncreaseStatItemAction increaseStatItem = itemAcquireAction as IncreaseStatItemAction;
            increaseStatItem.LoadPresistantStat();
        }
    }

}


