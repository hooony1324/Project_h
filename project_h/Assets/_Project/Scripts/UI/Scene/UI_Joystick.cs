using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public enum EJoystickState
{
    PointerDown,
    Drag,
    PointerUp
}

public class UI_Joystick : UI_Scene
{
    private HorizontalLayoutGroup layout;
    private GameObject _joystickCursor;
    private GameObject _joystickBG;
    private Vector2 _moveDir;
    private Vector2 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            OnRefreshShineBackground();
        }
    }
    private Vector2 _joystickTouchPos;
    private Vector2 _joystickOriginalPos;
    private float _radius;

    private enum GameObjects
    {
        Layout,
        JoystickBG,
        JoystickCursor,
        JoystickArea,
        TouchArea,
    }


    private enum ShineBGDirections
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        None,
    }
    private List<GameObject> _shineBG = new List<GameObject>();
    private ShineBGDirections _shineBGDirection;
    private ShineBGDirections ShineBGDirection
    {
        set
        {
            // 이미 세팅된 거면 안 건들고
            // 새로운 세팅이면 끄고 변경
            if (_shineBGDirection == value)
                return;

            if (_shineBGDirection != ShineBGDirections.None)
                _joystickBG.transform.GetChild((int)_shineBGDirection).gameObject.SetActive(false);

            _shineBGDirection = value;

            if (_shineBGDirection != ShineBGDirections.None)
                _joystickBG.transform.GetChild((int)_shineBGDirection).gameObject.SetActive(true);
        }
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindGameObjects(typeof(GameObjects));
       
        _joystickBG = GetGameObject((int)GameObjects.JoystickBG);
        _joystickCursor = GetGameObject((int)GameObjects.JoystickCursor);
        _joystickOriginalPos = _joystickBG.transform.position;

        GetGameObject((int)GameObjects.JoystickArea).BindEvent(null, OnJoystickDown, EUIEvent.BeginDrag);
        GetGameObject((int)GameObjects.JoystickArea).BindEvent(null, OnJoystickUp, EUIEvent.EndDrag);
        GetGameObject((int)GameObjects.JoystickArea).BindEvent(null, OnJoystickDrag, EUIEvent.Drag);

        GetGameObject((int)GameObjects.TouchArea).BindEvent(OnTouchDown, null, EUIEvent.PointerDown);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(OnTouchUp, null, EUIEvent.PointerUp);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(null, OnTouchDrag, EUIEvent.Drag);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingOrder = SortingLayers.JOYSTICK;

        foreach (Transform transform in _joystickBG.transform)
        {
            transform.gameObject.SetActive(false);
            _shineBG.Add(transform.gameObject);
        }

        IsVisible = false;

        return true;
    }

    void Start()
    {
        _radius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;
    }
    
    bool IsVisible
    {
        set
        {
            _joystickBG.SetActive(value);
            _joystickCursor.SetActive(value);
        } 
    }

    #region Event
    int touchCount = 0;
    private void OnJoystickDown(PointerEventData eventData)
    {
        IsVisible = true;

        _joystickTouchPos = eventData.position;
        Managers.Game.JoystickState = EJoystickState.PointerDown;

        Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(_joystickTouchPos);

        _joystickCursor.transform.position = worldTouchPos;
        _joystickBG.transform.position = worldTouchPos;

        Managers.Game.MoveDir = MoveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    private void OnJoystickDrag(PointerEventData eventData)
    {        
        Vector2 dragPos = eventData.position;

        MoveDir = (dragPos - _joystickTouchPos).normalized;

        float joystickDist = (dragPos - _joystickOriginalPos).sqrMagnitude;
        
        Vector2 clampedPos = _joystickTouchPos + MoveDir * Mathf.Clamp(joystickDist, 0, _radius);

        _joystickCursor.transform.position = Camera.main.ScreenToWorldPoint(clampedPos).With(z:0);
        
        Managers.Game.JoystickState = EJoystickState.Drag;
        Managers.Game.MoveDir = MoveDir;
    }

    private void OnJoystickUp(PointerEventData eventData)
    {
        MoveDir = Vector2.zero;

        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);

        _joystickCursor.transform.position = worldTouchPos;
        _joystickBG.transform.position = worldTouchPos;

        Managers.Game.MoveDir = MoveDir;
        Managers.Game.JoystickState = EJoystickState.PointerUp;

        IsVisible = false;
    }

    // 터치 후 드래그하는 방향에 따라 다른 스킬?
    // 탭 하는 시간에 따라 다른 스킬?
    private float tapThreshold = 0.14f;
    private float touchStartTime;
    
    private void OnTouchDown()
    {
        touchStartTime = Time.time;
    }

    private void OnTouchDrag(PointerEventData eventData)
    {        
    }

    private void OnTouchUp()
    {
        float tabTime = Time.time - touchStartTime;
        
        if (tabTime <= tapThreshold)
            Managers.Game.TriggerTab = tabTime;
    }

    private void OnRefreshShineBackground()
    {
        if (_moveDir.x < 0 && _moveDir.y > 0)
            ShineBGDirection = ShineBGDirections.TopLeft;
        else if (_moveDir.x > 0 && _moveDir.y > 0)
            ShineBGDirection = ShineBGDirections.TopRight;
        else if (_moveDir.x < 0 && _moveDir.y < 0)
            ShineBGDirection = ShineBGDirections.BottomLeft;
        else if (_moveDir.x > 0 && _moveDir.y < 0)
            ShineBGDirection = ShineBGDirections.BottomRight;
        else
            ShineBGDirection = ShineBGDirections.None;
    }
    #endregion
}
