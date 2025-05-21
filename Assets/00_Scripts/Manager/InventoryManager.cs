using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public Dictionary<string, Item> ItemList = new Dictionary<string, Item>();

    public void GetItem(ItemScriptable item)
    {
        if (ItemList.ContainsKey(item.name))
        {
            ItemList[item.name].count++;
        }
        else
        {
            ItemList.Add(item.name, new Item { data = item, count = 1 });
        }
    }
    
}
