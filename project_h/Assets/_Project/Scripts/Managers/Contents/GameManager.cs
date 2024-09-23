using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public class GameManager
{


    public void Init()
    {
        
    }


    private Vector2 _moveDir = Vector2.zero;
    public Vector2 MoveDir 
    {
        get { return _moveDir; }
        set 
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(value);
        }
    }

    private EJoystickState _joystickState;

    public EJoystickState JoystickState
    {
        get => _joystickState;
        set
        {
            _joystickState = value;
            OnJoystickStateChanged?.Invoke(_joystickState);
        }
    }
    
    public event Action<Vector2> OnMoveDirChanged;
    public EJoystickType JoystickType = EJoystickType.Flexible;
    public event Action<EJoystickState> OnJoystickStateChanged;

    private CameraController _cam;

    public CameraController Cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = Object.FindObjectOfType<CameraController>();
            }

            return _cam;
        }
    }
}
