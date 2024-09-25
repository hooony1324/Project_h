using UnityEngine;
using static Define;

public enum ECampState
{
    Idle,
    Move,
    MoveToTarget,
}

public class HeroCamp : BaseObject
{
    private Vector3 _moveDir { get; set; }
    public float Speed { get; set; } = 7f;
    public Transform Pivot { get; protected set; }
    public Transform Destination { get; protected set; }

    private ECampState _campState = ECampState.Idle;

    public ECampState CampState
    {
        get { return _campState; }
        set
        {
            {
                Debug.Log($"CampState : {value}");
                _campState = value;
            }
        }
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Pivot = Util.FindChild<Transform>(gameObject, "Pivot", true);
        Destination = Util.FindChild<Transform>(Pivot.gameObject, "Destination", true);

        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        return true;
    }
    
    private void Update()
    {
        Vector3 dir = _moveDir * (Time.deltaTime * Speed);
        Vector3 newPos = transform.position + dir;

        if (Managers.Map == null)
            return;
        if (Managers.Map.CanGo(this, newPos, ignoreObjects: true, ignoreSemiWall: true) == false)
            return;
        if (Managers.Game.Cam.State == ECameraState.Targeting)
            return;
        if(CampState == ECampState.MoveToTarget)
            return;

        transform.position = newPos;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;
        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(-dir.x, +dir.y) * 180 / Mathf.PI;

            if (angle > 15f && angle <= 75f)
                _moveDir = MoveDir.TOP_LEFT;
            else if (angle > 75f && angle <= 105f)
                _moveDir = MoveDir.LEFT;
            else if (angle > 105f && angle <= 160f)
                _moveDir = MoveDir.BOTTOM_LEFT;
            else if (angle > 160f || angle <= -160f)
                _moveDir = MoveDir.BOTTOM;
            else if (angle < -15f && angle >= -75f)
                _moveDir = MoveDir.TOP_RIGHT;
            else if (angle < -75f && angle >= -105f)
                _moveDir = MoveDir.RIGHT;
            else if (angle < -105f && angle >= -160f)
                _moveDir = MoveDir.BOTTOM_RIGHT;
            else
                _moveDir = MoveDir.TOP;

            Pivot.eulerAngles = new Vector3(0, 0, angle); //      
        }
    }

}
