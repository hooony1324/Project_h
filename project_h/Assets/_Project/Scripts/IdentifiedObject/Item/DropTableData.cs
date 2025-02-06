using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "DROPTABLEDATA", menuName = "Item/DropTableData")]
public class DropTableData : ScriptableObject
{
    [SerializeField] private int itemId;

    [SerializeField] private int groupId;
    
    [SerializeField] private float probability;

    public int ItemID => itemId;
    public int GroupID => groupId;
    public float Probability => probability;
}


