using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "DEFAULTSKILLFUSIONDATA", menuName = "AbilitySystem/DefaultSkillFusionData")]
public class DefaultSkillFusionData : ScriptableObject
{
    public int sourceSkillID;
    public SkillFusionData[] fusionDatas;
}

[System.Serializable]
public struct SkillFusionData
{
    public int targetSkillID;
    public int resultSkillID;
}
