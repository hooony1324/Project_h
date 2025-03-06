using System.Collections.Generic;
using System.Linq;

public class InventoryManager
{
    public delegate void ItemAddedHandler(Item item);
    public event ItemAddedHandler onItemAdded;

    private List<Item> allItems { get; } = new();
    private List<Item> equippedItems { get; } = new();
    public IReadOnlyList<Item> AllItems => allItems;
    public IReadOnlyList<Item> EquippedItems => equippedItems;

    public void AddItem(Item item)
    {
        if (!item.IsCombinedItem)
            allItems.Add(item);

        if (item.IsEquipment)
            equippedItems.Add(item);

        onItemAdded?.Invoke(item);
    }

    // 1개 이상 가지고 있는지 확인
    public bool IsMultiple(int itemID)
    {
        int count = AllItems.Count(x => x.ID == itemID);
        return count > 0;
    }


    public void LoadItemSaveData(IReadOnlyList<int> itemSaveDatas)
    {
        allItems.Clear();
        equippedItems.Clear();
        foreach (int itemID in itemSaveDatas)
        {
            Item item = Managers.Data.GetItemData(itemID);
            allItems.Add(item);
        }
    }

    public void LoadItems()
    {
        equippedItems.Clear();
        foreach (var item in allItems)
        {
            item.Acquire(bLoadSaveData:true);
        }
    }

    // 데이터 복구할 때만 사용
    public void AddEquippedItem(Item item)
    {
        equippedItems.Add(item);
    }

    public void RemoveEquippedItems(int[] itemIDs)
    {
        foreach (int itemID in itemIDs)
        {
            Item item = allItems.FirstOrDefault(x => x.ID == itemID);
            if (item != null)
                equippedItems.Remove(item);
        }
    }

    public void RemoveItem(Item item)
    {
        item.Release();

        allItems.Remove(item);
        equippedItems.Remove(item);
    }
}