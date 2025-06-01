using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUp_Handler : MonoBehaviour, IPointerDownHandler
{
    private Item_Scriptable item;

    public void Init(Item_Scriptable itemData)
    {
        item = itemData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BaseCanvas.instance.PopUpItem().Item_PopUp(item, eventData.position);
    }
}
