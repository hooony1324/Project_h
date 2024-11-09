using System;
using System.Threading;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Sensor : InitOnce
{
    public event Action OnTargetChanged = delegate { };

    [SerializeField] float _detectionRadius = 5f;
    [SerializeField] float _timerInterval = 1f;

    public Vector3 TargetPosition => _target ? _target.transform.position : Vector3.zero;
    public bool IsTargetInRange => TargetPosition != Vector3.zero;

    private CircleCollider2D _detectionRange;
    private GameObject _target;
    private Vector3 _lastKnownPosition;
    private CountdownTimer _timer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _detectionRange = GetComponent<CircleCollider2D>();
        _detectionRange.isTrigger = true;
        _detectionRange.radius = _detectionRadius;

        return true;
    }

    private void Start()
    {
        _timer = new CountdownTimer(_timerInterval);
        _timer.OnTimerStop += () => 
        {
            UpdateTargetPosition(_target ?? null);
            _timer.Start();
        };

        _timer.Start();
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        UpdateTargetPosition(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        UpdateTargetPosition();
    }

    private void UpdateTargetPosition(GameObject target = null)
    {
        _target = target;
        if (IsTargetInRange && (_lastKnownPosition != TargetPosition || _lastKnownPosition != Vector3.zero))
        {
            _lastKnownPosition = TargetPosition;
            OnTargetChanged.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTargetInRange ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

}