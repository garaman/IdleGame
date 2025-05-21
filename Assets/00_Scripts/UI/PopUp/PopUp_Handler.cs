using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUp_Handler : MonoBehaviour, IPointerDownHandler
{
    private ItemScriptable item;

    public void Init(ItemScriptable itemData)
    {
        item = itemData;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BaseCanvas.instance.PopUpItem().Item_PopUp(item, eventData.position);
    }
}
