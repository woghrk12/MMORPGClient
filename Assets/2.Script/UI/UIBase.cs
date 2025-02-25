using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIEventHandler))]
public abstract class UIBase : MonoBehaviour
{
    #region Variables

    protected Dictionary<Type, UnityEngine.Object[]> objectDictionary = new();

    #endregion Variables

    #region Methods

    public static void BindEvent(GameObject go, Action<PointerEventData> action, EUIEvent eventType = EUIEvent.CLICK)
    {
        UIEventHandler handler = go.GetOrAddComponent<UIEventHandler>();

        switch (eventType)
        {
            case EUIEvent.CLICK:
                handler.ClickHandler -= action;
                handler.ClickHandler += action;
                break;

            case EUIEvent.BEGIN_DRAG:
                handler.BeginDragHandler -= action;
                handler.BeginDragHandler += action;
                break;

            case EUIEvent.DRAG:
                handler.DragHandler -= action;
                handler.DragHandler += action;
                break;
        }
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        int length = names.Length;

        UnityEngine.Object[] objects = new UnityEngine.Object[length];

        objectDictionary.Add(typeof(T), objects);

        for (int index = 0; index < length; index++)
        {
            if (!Utility.FindChild<T>(gameObject, out T component, names[index], true))
            {
                Debug.LogWarning($"Failed to find the object.\nType : {typeof(T)} / Name : {names[index]}");
                continue;
            }

            objects[index] = component;
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        if (!objectDictionary.TryGetValue(typeof(T), out UnityEngine.Object[] objects)) return null;
        if (index < 0 || index >= objects.Length) return null;

        return objects[index] as T;
    }

    #endregion Methods
}
