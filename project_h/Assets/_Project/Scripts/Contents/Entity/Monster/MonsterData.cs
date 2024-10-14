using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "GameDesign/MonsterData")]
public class MonsterData : EntityData
{
    [SerializeField]
    private string animatorControllerName;

    [SerializeField]
    private StatOverride[] statsForOverride; // => Override 할 스탯들임
}