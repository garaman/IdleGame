using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public void GetItem(string itemName, int value = 1)
    {

        if (BaseManager.Data.ItemInfos.ContainsKey(itemName))
        {
            BaseManager.Data.ItemInfos[itemName].Count += value;
        }
        else
        {
            Info iteminfo = new Info();
            iteminfo.Count = value;

            BaseManager.Data.ItemInfos.Add(itemName, iteminfo);            
        }
    }
    
}
