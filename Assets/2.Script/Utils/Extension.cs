using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, EUIEvent eventType = EUIEvent.CLICK)
    {
        UIBase.AddUIEvent(go, action, eventType);
    }
}
