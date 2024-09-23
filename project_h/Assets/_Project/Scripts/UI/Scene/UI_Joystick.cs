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
public enum EJoystickType
{
    Fixed,
    Flexible
}

public class UI_Joystick : UI_Scene
{
    private GameObject _handler;
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
        _handler = GetGameObject((int)GameObjects.JoystickCursor);
        _joystickOriginalPos = _joystickBG.transform.position;
        gameObject.BindEvent(OnPointerDown, EUIEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, EUIEvent.PointerUp);
        gameObject.BindEvent(OnDrag, EUIEvent.Drag);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = Camera.main;

        return true;
    }

    void Start()
    {
        _radius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;

    }
    
    
    #region Event
    private void OnPointerDown(PointerEventData eventData)
    {
        _joystickTouchPos = eventData.position;
        Managers.Game.JoystickState = EJoystickState.PointerDown;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_joystickTouchPos);

        if (Managers.Game.JoystickType == EJoystickType.Flexible)
        {
            _handler.transform.position = mouseWorldPos;
            _joystickBG.transform.position = mouseWorldPos;


        }

        //(Managers.UI.SceneUI as UI_GameScene).HideUIOnMove();

        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }

    private void OnDrag(PointerEventData eventData)
    {        
        Vector2 dragePos = eventData.position;

        _moveDir = Managers.Game.JoystickType == EJoystickType.Fixed
            ? (dragePos - _joystickOriginalPos).normalized
            : (dragePos - _joystickTouchPos).normalized;

        // 조이스틱이 반지름 안에 있는 경우
        float joystickDist = (dragePos - _joystickOriginalPos).sqrMagnitude;

        Vector3 newPos;
        // 조이스틱이 반지름 안에 있는 경우
        if (joystickDist < _radius)
        {
            newPos = _joystickTouchPos + _moveDir * joystickDist;
        }
        else // 조이스틱이 반지름 밖에 있는 경우
        {
            newPos = Managers.Game.JoystickType == EJoystickType.Fixed
                ? _joystickOriginalPos + _moveDir * _radius
                : _joystickTouchPos + _moveDir * _radius;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(newPos);

        _handler.transform.position = worldPos;


        Managers.Game.JoystickState = EJoystickState.Drag;
        Managers.Game.MoveDir = _moveDir;
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_joystickOriginalPos);

        _handler.transform.position = worldPos;
        _joystickBG.transform.position = worldPos;
        Managers.Game.MoveDir = _moveDir;
        Managers.Game.JoystickState = EJoystickState.PointerUp;

    }
    #endregion
}
