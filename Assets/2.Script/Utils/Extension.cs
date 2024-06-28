using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Utility.GetOrAddComponent<T>(go);
    }

    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, EUIEvent eventType = EUIEvent.CLICK)
    {
        UIBase.AddUIEvent(go, action, eventType);
    }
}
