using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        GetGameObject((int)GameObjects.JoystickArea).BindEvent(OnJoystickDown, EUIEvent.PointerDown);
        GetGameObject((int)GameObjects.JoystickArea).BindEvent(OnJoystickUp, EUIEvent.PointerUp);
        GetGameObject((int)GameObjects.JoystickArea).BindEvent(OnJoystickDrag, EUIEvent.Drag);

        GetGameObject((int)GameObjects.TouchArea).BindEvent(OnTouchDown, EUIEvent.PointerDown);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(OnTouchUp, EUIEvent.PointerUp);
        GetGameObject((int)GameObjects.TouchArea).BindEvent(OnTouchDrag, EUIEvent.Drag);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
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

        _joystickCursor.transform.position = _joystickTouchPos;
        _joystickBG.transform.position = _joystickTouchPos;


        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    private void OnJoystickDrag(PointerEventData eventData)
    {        
        Vector2 dragePos = eventData.position;

        _moveDir = (dragePos - _joystickTouchPos).normalized;

        float joystickDist = (dragePos - _joystickOriginalPos).sqrMagnitude;
        Vector2 newCursorPos = _joystickTouchPos + _moveDir * Mathf.Clamp(joystickDist, 0, _radius);
        _joystickCursor.transform.position = newCursorPos;

        Managers.Game.JoystickState = EJoystickState.Drag;
        Managers.Game.MoveDir = _moveDir;
    }

    private void OnJoystickUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;

        _joystickCursor.transform.position = _joystickTouchPos;
        _joystickBG.transform.position = _joystickTouchPos;

        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.PointerUp;

        IsVisible = false;
    }

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
