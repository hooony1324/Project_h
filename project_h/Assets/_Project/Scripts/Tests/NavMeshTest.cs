using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    static float agentDrift = 0.0001f; // minimal
    void Update()
    {
        if (Target == null)
            return;

        Vector3 driftPos = Target.transform.position + new Vector3(agentDrift, 0f, 0f);
        agent.SetDestination(driftPos);
        
    }
}
