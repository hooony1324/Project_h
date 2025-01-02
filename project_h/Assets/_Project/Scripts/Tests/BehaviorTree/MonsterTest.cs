using System;
using Unity.Behavior;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    public BehaviorGraphAgent behaviorAgent;

    void Awake()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
    }

    void Start()
    {
        
    }

}