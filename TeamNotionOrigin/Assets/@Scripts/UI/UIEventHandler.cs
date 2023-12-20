using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public Action<PointerEventData> ClickAction;
    public Action<PointerEventData> HoverAction;
    public Action<PointerEventData> DetachAction;
    public Action<PointerEventData> DragAction;

    public void OnDrag(PointerEventData eventData)
    {
        if (DragAction != null)
        {
            DragAction.Invoke(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ClickAction != null)
        {
            ClickAction.Invoke(eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverAction != null)
        {
            HoverAction.Invoke(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DetachAction != null)
        {
            DetachAction.Invoke(eventData);
        }
    }
}
