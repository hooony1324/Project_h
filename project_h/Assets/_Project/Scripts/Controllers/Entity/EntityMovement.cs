using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Entity Owner { get; private set; }
    private Stat entityMoveSpeedStat;
    public void Setup(Entity owner)
    {
        Owner = owner;

        entityMoveSpeedStat = Owner.Stats.MoveSpeedStat ?? Owner.Stats.GetStat("MOVESPEED");
    }

    // 조이스틱에 의한 강제 이동
    public bool IsForcedMoving {get; set;}

    public float IsMoving => !Owner.LerpCellPosCompleted ? 1 : 0;

    public void StartTrace()
    {
        StartCoroutine(TraceUpdate());
    }

    public void StopTrace()
    {
        StopCoroutine(TraceUpdate());
    }

    IEnumerator TraceUpdate()
    {
        while (true)
        {
            Owner.LerpToCellPos(entityMoveSpeedStat.Value);
            yield return null;
        }
    }

    #region GridWorld Moving
    public EFindPathResult FindPathAndMoveToCellPos(Vector3 destWorldPos, int maxDepth, bool forceMoveCloser = false)
    {
        Vector3Int destCellPos = Managers.Map.World2Cell(destWorldPos);
        return FindPathAndMoveToCellPos(destCellPos, maxDepth, forceMoveCloser);
    }

    private List<Vector3Int> _debugPath = new List<Vector3Int>();

    public EFindPathResult FindPathAndMoveToCellPos(Vector3Int destCellPos, int maxDepth, bool forceMoveCloser = false)
    {
        if (Owner.CellPos == destCellPos)
        {
            return EFindPathResult.SamePosition;
        }

        if (Owner.LerpCellPosCompleted == false)
            return EFindPathResult.Fail_LerpCell;

        // A*
        List<Vector3Int> path = Managers.Map.FindPath(Owner, Owner.CellPos, destCellPos, maxDepth, Owner.ExtraCells);
        if (path.Count < 2)
            return EFindPathResult.Fail_NoPath;

        _debugPath = path; // 여기서 경로를 _debugPath에 저장

        if (forceMoveCloser)
        {
            Vector3Int diff1 = Owner.CellPos - destCellPos;
            Vector3Int diff2 = path[1] - destCellPos;
            if (diff1.sqrMagnitude <= diff2.sqrMagnitude)
                return EFindPathResult.Fail_NoPath;
        }

        Vector3Int dirCellPos = path[1] - Owner.CellPos;
        Vector3Int nextPos = Owner.CellPos + dirCellPos;

        if (Managers.Map.MoveTo(Owner, nextPos) == false)
            return EFindPathResult.Fail_MoveTo;

        return EFindPathResult.Success;
    }
    #endregion
}