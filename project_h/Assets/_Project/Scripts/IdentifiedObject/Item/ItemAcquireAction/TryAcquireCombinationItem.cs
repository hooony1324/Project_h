using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class TryAcquireCombinationItem : ItemAcquireAction
{
    [SerializeField] private int resultItemID;
    public override void AqcuireAction(Item owner)
    {
        this.owner = owner;

        if(Managers.Inventory.IsMultiple(resultItemID))
            return;

        // combination확인
        if (Managers.Data.ItemCombinationDatas.TryGetValue(resultItemID, out int[] sourceItemIDs))
        {
            // 모든 재료 아이템 가지고 있는지 확인   
            if (sourceItemIDs.All(id => Managers.Inventory.EquippedItems.Any(x => x.ID == id)))
            {
                // 인벤토리에 재료 아이템 슬릇 삭제
                Managers.Inventory.RemoveEquippedItems(sourceItemIDs);

                // 완성 아이템 획득
                Item combinationItem = Managers.Data.GetItemData(resultItemID);
                combinationItem.Acquire();
                Managers.Inventory.AddItem(combinationItem);
            }

            // 조합 실패 (모든 조합 아이템 가지고 있는 조건 충족안됨)

        }

        // 실패 (테이블에 resultItemID 데이터 없음)
    }
}