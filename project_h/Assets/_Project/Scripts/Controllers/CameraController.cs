using DG.Tweening;
using UnityEngine;

public enum ECameraState
{
    Following,
    Targeting,
}

public class CameraController : MonoBehaviour
{
    public ECameraState State { get; set; }
    private bool isReady = false;
    private Entity _target;
    public Entity Target
    {
        get { return _target; }
        set
        {
            _target = value;
            isReady = true;
        }
    }

    [SerializeField] public float smoothSpeed = 6f;
    private int _targetOrthographicSize = 15;

    public void Start()
    {
        State = ECameraState.Following;
    }

    private void LateUpdate()
    {
        // Smoothly transition to the target camera size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _targetOrthographicSize, smoothSpeed * Time.deltaTime);
        
        HandleCameraPosition();
    }

    private void HandleCameraPosition()
    {
        if (isReady == false || State == ECameraState.Targeting)
            return;

        Vector3 targetPosition = new Vector3(Target.Position.x, Target.Position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
    }
    public void TargetingCamera(Entity dest)
    {
        //이미 진행중이면 리턴
        if (State == ECameraState.Targeting)
            return;

        State = ECameraState.Targeting;
        Vector3 targetPosition = new Vector3(Target.CenterPosition.x, Target.CenterPosition.y, -10f);
        Vector3 destPosition = new Vector3(dest.Position.x, dest.Position.y, -10f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(destPosition, 0.8f).SetEase(Ease.Linear))
            .AppendInterval(2f)
            .Append(transform.DOMove(targetPosition, 0.8f).SetEase(Ease.Linear))
            .OnComplete(() => { State = ECameraState.Following; });
    }
}