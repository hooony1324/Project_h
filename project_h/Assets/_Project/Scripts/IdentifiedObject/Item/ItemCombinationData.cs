using UnityEngine;

[CreateAssetMenu(fileName = "ITEMCOMBINATION", menuName = "Item/ItemCombination")]
[System.Serializable]
public class ItemCombination : ScriptableObject
{
    [SerializeField] int resultItemID;
    [SerializeField] int[] sourceItemIDs;
}

