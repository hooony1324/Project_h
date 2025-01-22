using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "DROPTABLEDATA", menuName = "Item/DropTableData")]
public class DropTableData : ScriptableObject
{
    [SerializeField] private int id;

    [SerializeField] private DropData[] dropDatas;

    public int ID => id;
    public List<DropData> DropList => dropDatas.ToList();
}


[System.Serializable]
public class DropData
{
    public int itemID;
    public int probability;    // 10 -> 10퍼의 확률로 드랍

    // public int count
}