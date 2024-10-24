using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField]
    private Sprite sprite;
    
    [SerializeField]
    private float scale = 1;

    public Sprite Sprite => sprite;
}