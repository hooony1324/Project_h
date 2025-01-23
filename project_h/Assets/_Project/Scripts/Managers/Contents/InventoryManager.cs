using System.Collections.Generic;
using System.Linq;

public class InventoryManager
{
    public delegate void ItemAddedHandler(Item item);
    public event ItemAddedHandler onItemAdded;

    public List<Item> allItems { get; } = new();
    public IReadOnlyList<Item> AllItems => allItems;

    public void AddItem(Item item)
    {
        allItems.Add(item);
        onItemAdded?.Invoke(item);
    }

    // 1개 이상 가지고 있는지 확인
    public bool IsMultiple(int itemID)
    {
        return AllItems.Select(x => x.ID == itemID).Count() > 0;
    }


    public void LoadItemSaveData(IReadOnlyList<int> itemSaveDatas)
    {
        allItems.Clear();
        foreach (int itemID in itemSaveDatas)
        {
            Item item = Managers.Data.GetItemData(itemID);
            allItems.Add(item);
        }
    }

    public void LoadItems()
    {
        foreach (var item in allItems)
        {
            item.Load();
        }
    }
}