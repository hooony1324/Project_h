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
    [SerializeField, Tooltip("아이템 조합 가능 여부")] private bool isCombinable = true;


    [SerializeReference, SubclassSelector] public ItemAcquireAction[] itemAcquireActions;
    
    [SerializeField] private string description;
    
    public int ID => id;
    public Sprite ItemHolderSprite => itemHolderSprite;
    public bool IsAllowMultiple => isAllowMultiple;
    public bool IsEquipment => isEquipment;
    public bool IsSpawnable
    {
        get
        {
            if (itemAcquireActions.Length == 0)
                return false;
                
           return itemAcquireActions.All(x => x.IsSpawnable);
        }
    }  

    public void Acquire()
    {
        foreach (var itemAcquireAction in itemAcquireActions)
        {
            itemAcquireAction.AqcuireAction(this);
        }
    }

    public void Release()
    {
        foreach (var itemAcquireAction in itemAcquireActions)
        {
            itemAcquireAction.Release();
        }
    }

    public void Load()
    {
        foreach (var itemAcquireAction in itemAcquireActions)
        {
            if (!itemAcquireAction.IsActionType<IncreaseStatItemAction>())
                continue;

            IncreaseStatItemAction increaseStatItem = itemAcquireAction as IncreaseStatItemAction;
            increaseStatItem.LoadStat();
        }
    }

}


