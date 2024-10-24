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
    private Vector2 _moveDir { get; set; }
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

        GetGameObject((int)GameObjects.TouchArea).BindEvent(null, OnTouchDown, EUIEvent.BeginDrag);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(null, OnTouchUp, EUIEvent.EndDrag);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(null, OnTouchDrag, EUIEvent.Drag);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingOrder = SortingLayers.JOYSTICK;

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

        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    private void OnJoystickDrag(PointerEventData eventData)
    {        
        Vector2 dragePos = eventData.position;

        _moveDir = (dragePos - _joystickTouchPos).normalized;

        float joystickDist = (dragePos - _joystickOriginalPos).sqrMagnitude;
        
        Vector2 clampedPos = _joystickTouchPos + _moveDir * Mathf.Clamp(joystickDist, 0, _radius);

        _joystickCursor.transform.position = Camera.main.ScreenToWorldPoint(clampedPos).With(z:0);
        
        Managers.Game.JoystickState = EJoystickState.Drag;
        Managers.Game.MoveDir = _moveDir;
    }

    private void OnJoystickUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;

        Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(eventData.position);

        _joystickCursor.transform.position = worldTouchPos;
        _joystickBG.transform.position = worldTouchPos;

        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.PointerUp;

        IsVisible = false;
    }

    // 터치 후 드래그하는 방향에 따라 다른 스킬?
    // 탭 하는 시간에 따라 다른 스킬?
    private float tapThreshold = 0.14f;
    private float touchStartTime;
    
    private void OnTouchDown(PointerEventData eventData)
    {
        touchStartTime = Time.time;
    }

    private void OnTouchDrag(PointerEventData eventData)
    {        
    }

    private void OnTouchUp(PointerEventData eventData)
    {
        float tabTime = Time.time - touchStartTime;
        
        if (tabTime <= tapThreshold)
            Managers.Game.TriggerTab = tabTime;
    }
    #endregion
}
