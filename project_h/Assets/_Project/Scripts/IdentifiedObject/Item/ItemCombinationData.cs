using UnityEngine;

[CreateAssetMenu(fileName = "ITEMCOMBINATIONDATA", menuName = "Item/ItemCombinationData")]
[System.Serializable]
public class ItemCombinationData : ScriptableObject
{
    [SerializeField] int resultItemID;
    [SerializeField] int[] sourceItemIDs;

    public int ResultItemID => resultItemID;
    public int[] SourceItemIDs => sourceItemIDs;
}

