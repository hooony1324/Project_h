using UnityEngine;


#if UNITY_EDITOR
public class GizmoSpawner : MonoBehaviour
{
    public static GizmoSpawner Instance 
    { 
        get
        {
            if (_instance == null)
                _instance = new GameObject("@GizmoSpawner").AddComponent<GizmoSpawner>();
            return _instance;
        }
    }

    private static GizmoSpawner _instance;

    private Vector3 _lastDebugPoint;
    private Vector2 _lastDebugBoxSize;
    private float _lastDebugAngle;
    private bool _isDebugDataValid;
    private CountdownTimer _debugTimer;

    private void Awake()
    {
        _debugTimer = new CountdownTimer(0f);

        DontDestroyOnLoad(gameObject);
    }

    public void DrawDebugBox(Vector3 point, Vector2 boxSize, float angle, float duration)
    {
        _lastDebugPoint = point;
        _lastDebugBoxSize = boxSize;
        _lastDebugAngle = angle;
        _isDebugDataValid = true;
        
        _debugTimer.OnTimerStop -= OnDebugTimerStop;
        _debugTimer.OnTimerStop += OnDebugTimerStop;
        _debugTimer.Reset(duration);
        _debugTimer.Start();
    }

    private void OnDebugTimerStop()
    {
        _isDebugDataValid = false;
    }

    private void Update()
    {
        _debugTimer.Tick(Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!_isDebugDataValid)
        {
            return;
        }
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        
        Matrix4x4 rotationMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(_lastDebugPoint, Quaternion.Euler(0, 0, _lastDebugAngle), Vector3.one);
        
        Gizmos.DrawCube(Vector3.zero, _lastDebugBoxSize);
        
        Gizmos.matrix = rotationMatrix;
    }
}
#endif