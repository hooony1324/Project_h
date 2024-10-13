using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "MonsterData", order = 0)]
public class MonsterData : EntityData
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private string animatorControllerName;

    [SerializeField]
    private float scale = 1;

    [SerializeField]
    private StatOverride[] statsForOverride; // => Override 할 스탯들임
}