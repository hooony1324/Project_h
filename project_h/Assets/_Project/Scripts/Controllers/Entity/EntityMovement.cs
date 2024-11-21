using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;


public enum EFindPathResult
{
    Fail_LerpCell,
    Fail_NoPath,
    Fail_MoveTo,
    Success,
    SamePosition,//같은좌표에서 길찾기했을때
}

public class EntityMovement : MonoBehaviour
{
    public delegate void SetDestinationHandler(EntityMovement movement, Vector3 destination);
    public event SetDestinationHandler onSetDestination;

    public Entity Owner { get; private set; }
    public bool IsAgentMoving => agent.velocity.sqrMagnitude > 0f;

    // 조이스틱에 의한 강제 이동
    public bool IsForcedMoving {get; set;}

    public bool AgentEnabled
    {
        set
        {
            agent.enabled = value;
        }
    }

    private NavMeshAgent agent;
    private Stat entityMoveSpeedStat;
    private Transform traceTarget;
    
    [SerializeField]
    private float rollTime = 0.5f;

    public Transform TraceTarget
    {
        get => traceTarget;
        set
        {
            if (traceTarget == value)
                return;
            
            Stop();
            
            traceTarget = value;
            if (traceTarget)
                StartCoroutine("TraceUpdate");
        }
    }
    public Vector3 Destination
    {
        get => agent.destination;
        set
        {
            // traceTarget을 추적하는 것을 멈춤
            TraceTarget = null;

            if (agent.enabled == false)
                return;
                
            if (Owner.IsDead)
                return;

            SetDestination(value);
        }
    }

    public bool AtDestination => agent.destination.InRangeOf(transform.position, 0.2f);

    public void Move(Vector3 moveDir)
    {
        if (IsRolling)
            return;

        Vector3 nextPos = transform.position + moveDir;
        NavMesh.Raycast(transform.position, nextPos, out NavMeshHit hit, NavMesh.AllAreas);
        
        if (hit.distance > 0.4f)
            Destination = nextPos;
    }

    public bool IsRolling { get; private set; }
    
    public void Setup(Entity owner)
    {
        Owner = owner;

        agent = Owner.GetComponent<NavMeshAgent>();

        entityMoveSpeedStat = Owner.Stats.MoveSpeedStat ?? Owner.Stats.GetStat("MOVESPEED");
        if (entityMoveSpeedStat)
        {
            agent.speed = entityMoveSpeedStat.Value;
            entityMoveSpeedStat.onValueChanged += OnMoveSpeedChanged;
        }
    }

    private void OnDisable() => Stop();

    private void OnDestroy()
    {
        if (entityMoveSpeedStat)
            entityMoveSpeedStat.onValueChanged -= OnMoveSpeedChanged;
    }

    private void OnMoveSpeedChanged(Stat stat, float currentValue, float preValue)
        => agent.speed = currentValue;

    private void SetDestination(Vector3 destination)
    {
        if (Owner.Target != null && Owner.Target.IsDead)
        {
            traceTarget = null;
            return;
        }

        agent.destination = destination;
        LookAt(destination);

        onSetDestination?.Invoke(this, destination);
    }

    public void Stop()
    {
        traceTarget = null;
        StopCoroutine("TraceUpdate");

        if (agent.isOnNavMesh)
            agent.ResetPath();

        agent.velocity = Vector3.zero;
    }

    public void LookAt(Vector3 position)
    {
        Owner.LookAt(position);
    }
 
    Vector3 rollDirection;
    public void Roll(float distance)
        => Roll(distance, agent.velocity.normalized);
    public void Roll(float distance, Vector3 direction)
    {
        Stop();

        var animator = Owner.Animator;
        if (animator)
            animator.SetFloat("rollSpeed", 1 / rollTime);

        rollDirection = direction;
        // +a) agent끄고(회피는 유닛 지나가게) NavMesh로 distance계산
        Vector3 expectedRollPosition = transform.position + rollDirection * distance;
        NavMesh.Raycast(transform.position, expectedRollPosition, out NavMeshHit hit, NavMesh.AllAreas);

        
        distance -= (expectedRollPosition - hit.position).magnitude;

        // 바라볼 방향 바라봄
        if (direction != Vector3.zero)
            LookAt(expectedRollPosition);
        
        StopCoroutine("RollUpdate");
        StartCoroutine("RollUpdate", distance);
    }

    private IEnumerator RollUpdate(float distance)
    {
        IsRolling = true;
        agent.enabled = false;
        
        // 현재까지 구른 시간
        float currentRollTime = 0f;
        // 이전 Frame에 이동한 거리
        float prevRollDistance = 0f;

        while (true)
        {
            currentRollTime += Time.deltaTime;

            float timePoint = currentRollTime / rollTime;
            float inOutSine = -(Mathf.Cos(Mathf.PI * timePoint) - 1f) / 2f;
            float currentRollDistance = Mathf.Lerp(0f, distance, inOutSine);

            float deltaValue = currentRollDistance - prevRollDistance;

            transform.position += (rollDirection * deltaValue);
            prevRollDistance = currentRollDistance;

            if (currentRollTime >= rollTime)
                break;
            else
                yield return null;
        }

        IsRolling = false;
        agent.enabled = true;
    }

    private IEnumerator TraceUpdate()
    {
        while (true)
        {
            if (Vector3.SqrMagnitude(TraceTarget.position - transform.position) > 1.0f)
            {
                SetDestination(TraceTarget.position);
                yield return null;
            }
            else
                break;
        }
    }
}