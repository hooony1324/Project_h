using System.Collections.Generic;
using System.Linq;

public class InventoryManager
{
    // 중복된 아이템 없음
    public List<int> AllItems { get; } = new();


    // 1개 이상 가지고 있는지 확인
    public bool IsMultiple(int itemID)
    {
        return AllItems.Select(x => x == itemID).Count() > 0;
    }


    public void LoadItemSaveData(IReadOnlyList<int> itemSaveDatas)
    {
        AllItems.Clear();
        foreach (int itemID in itemSaveDatas)
        {
            AllItems.Add(itemID);
        }
    }

    public void LoadItems()
    {
        foreach (int itemID in AllItems)
        {
            Item item = Managers.Data.GetItemData(itemID);
            if (item != null)
                item.Load();
        }
    }
}