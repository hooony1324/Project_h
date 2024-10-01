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
    private GameObject _joystickCursor;
    private GameObject _joystickBG;
    private Vector2 _moveDir { get; set; }
    private Vector2 _joystickTouchPos;
    private Vector2 _joystickOriginalPos;
    private float _radius;

    private enum GameObjects
    {
        JoystickBG,
        JoystickCursor,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindGameObjects(typeof(GameObjects));
       
        _joystickBG = GetGameObject((int)GameObjects.JoystickBG);
        _joystickCursor = GetGameObject((int)GameObjects.JoystickCursor);
        _joystickOriginalPos = _joystickBG.transform.position;
        gameObject.BindEvent(OnPointerDown, EUIEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, EUIEvent.PointerUp);
        gameObject.BindEvent(OnDrag, EUIEvent.Drag);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        IsVisible = false;

        return true;
    }

    void Start()
    {
        _radius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;
        //GetComponent<Canvas>().worldCamera = Managers.Game.Cam.UICamera;
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
    private void OnPointerDown(PointerEventData eventData)
    {
        IsVisible = true;

        _joystickTouchPos = eventData.position;
        Managers.Game.JoystickState = EJoystickState.PointerDown;

        _joystickCursor.transform.position = _joystickTouchPos;
        _joystickBG.transform.position = _joystickTouchPos;


        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    private void OnDrag(PointerEventData eventData)
    {        
        Vector2 dragePos = eventData.position;

        _moveDir = (dragePos - _joystickTouchPos).normalized;

        float joystickDist = (dragePos - _joystickOriginalPos).sqrMagnitude;
        Vector2 newCursorPos = _joystickTouchPos + _moveDir * Mathf.Clamp(joystickDist, 0, _radius);
        _joystickCursor.transform.position = newCursorPos;

        Managers.Game.JoystickState = EJoystickState.Drag;
        Managers.Game.MoveDir = _moveDir;
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;

        _joystickCursor.transform.position = _joystickTouchPos;
        _joystickBG.transform.position = _joystickTouchPos;

        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.PointerUp;

        IsVisible = false;
    }
    #endregion
}
