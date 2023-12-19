using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour {

    protected Dictionary<Type, UnityEngine.Object[]> _objects = new();

    private bool _initialized;

    protected virtual void OnEnable() {
        Initialize();
    }

    public virtual bool Initialize() {
        if (_initialized) return false;

        _initialized = true;
        return true;
    }

    protected virtual void SetOrder() {

    }

    private void Bind<T>(Type type) where T : UnityEngine.Object {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        for (int i = 0; i < names.Length; i++)
            objects[i] = typeof(T) == typeof(GameObject) ? this.gameObject.FindChild(names[i]) : this.gameObject.FindChild<T>(names[i]);

        _objects.Add(typeof(T), objects);
    }
    protected void BindObject(Type type) => Bind<GameObject>(type);
    protected void BindText(Type type) => Bind<TextMeshProUGUI>(type);
    protected void BindButton(Type type) => Bind<Button>(type);
    protected void BindImage(Type type) => Bind<Image>(type);

    private T Get<T>(int index) where T : UnityEngine.Object {
        if (!_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objs)) return null;
        return objs[index] as T;
    }
    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected TextMeshProUGUI GetText(int index) => Get<TextMeshProUGUI>(index);
    protected Button GetButton(int index) => Get<Button>(index);
    protected Image GetImage(int index) => Get<Image>(index);


    protected void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent uIEvent)
    {
        UIEventHandler uiEventHandler = Utilities.GetOrAddComponent<UIEventHandler>(go);


        switch (uIEvent)
        {
            case Define.UIEvent.Click:
                uiEventHandler.ClickAction -= action;
                uiEventHandler.ClickAction += action;
                break;
            case Define.UIEvent.Hover:
                uiEventHandler.HoverAction -= action;
                uiEventHandler.HoverAction += action;
                break;
            case Define.UIEvent.Detach:
                uiEventHandler.DetachAction -= action;
                uiEventHandler.DetachAction += action;
                break;
        }
    }
}