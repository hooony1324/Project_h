using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Base : InitOnce
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.LogError($"Failed to Bind : {names[i]}");
            }
        }
    }

    protected void BindGameObjects(Type type) { Bind<GameObject>(type); }
    protected void BindTexts(Type type) { Bind<Text>(type); }
    protected void BindImages(Type type) { Bind<Image>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    protected void BindToggles(Type type) { Bind<Toggle>(type); }
    protected void BindSliders(Type type) { Bind<Slider>(type); }
    protected void BindTMPTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindTMPDropdowns(Type type) { Bind<TMP_Dropdown>(type); }
    protected void BindTMPInputFields(Type type) { Bind<TMP_InputField>(type); }

    protected T Get<T>(int idx) where T : Object
    {
        Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        return objects[idx] as T;
    }
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
    protected TMP_Text GetTMPText(int idx) { return Get<TMP_Text>(idx); }
    protected TMP_Dropdown GetTMPDropdown(int idx) { return Get<TMP_Dropdown>(idx); }
    protected TMP_InputField GetTMPInputField(int idx) { return Get<TMP_InputField>(idx); }

    public static void BindEvent(GameObject go, Action action = null, Action<PointerEventData> dragAction = null, EUIEvent type = EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case EUIEvent.Click:
                //Util.GetOrAddComponent<UI_ButtonAnimation>(go);
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case EUIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case EUIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case EUIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case EUIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                evt.OnDragHandler += dragAction;
                break;
            case EUIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                evt.OnBeginDragHandler += dragAction;
                break;
            case EUIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                evt.OnEndDragHandler += dragAction;
                break;
        }
    }
}
