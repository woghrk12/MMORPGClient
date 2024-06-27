using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EUIEvent
{ 
    CLICK,
    BEGIN_DRAG,
    DRAG,
}

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
    #region Variables

    public event Action<PointerEventData> ClickHandler = null;
    public event Action<PointerEventData> BeginDragHandler = null;
    public event Action<PointerEventData> DragHandler = null;

    #endregion Variables

    #region Interface Implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickHandler?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragHandler?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragHandler?.Invoke(eventData);
    }

    #endregion Interface Implementation
}
