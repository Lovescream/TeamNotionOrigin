using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //a
        GameObject dropped = eventData.pointerDrag;
        //a
        ItemDragAndDrop a = dropped.GetComponent<ItemDragAndDrop>();
        if (transform.childCount == 0)
        { 
            a.parentAfterDrag = transform;
        }
        else
        {
            //b
            ItemDragAndDrop b = transform.GetChild(0).GetComponent<ItemDragAndDrop>();

            //a
            a.parentAfterDrag = b.transform.parent;

            //b
            b.parentAfterDrag = a.transform.parent;

            // 슬롯 자체의 위치를 교체
            //dropped.transform.SetParent(transform);
            //draggedIte.transform.SetParent(draggedItem.parentAfterDrag);
        }
    }
}
