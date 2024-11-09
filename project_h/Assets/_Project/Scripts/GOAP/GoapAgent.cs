using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class GoapAgent : MonoBehaviour
{
    [Header("Sensors")]
    [SerializeField] Sensor _chaseSensor;
    [SerializeField] Sensor _attackSensor;

    [Header("Known Locations")]
    [SerializeField] Transform _restingPosition;
    
    NavMeshAgent _agent;
    
}   