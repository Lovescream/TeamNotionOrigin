using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemDragAndDrop draggedItem = dropped.GetComponent<ItemDragAndDrop>();

        //드래그중인 오브젝트의 부모를 저장
        Transform tempTransform = draggedItem.parentAfterDrag;
        Transform currentTransform = transform.GetChild(0);
        if (transform.childCount == 0)
        {
            draggedItem.parentAfterDrag = transform;
        }
        else
        {
            draggedItem.parentAfterDrag = transform;
            currentTransform.parent = tempTransform;
        }
    }
}
