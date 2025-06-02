using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager 
{
    public List<Item_Scriptable> GetDropSet()
    {
        List<Item_Scriptable> dropSet = new List<Item_Scriptable>();

        foreach (var item in BaseManager.Data.ItemData)
        {
            if(item.Value.ItemType == ItemType.Consumable)
            {
                float valueCount = Random.Range(0f, 100.0f);
                if (valueCount <= item.Value.ItemChance)
                {
                    dropSet.Add(item.Value);
                }
            }            
        }

        return dropSet;
    }

    public void GetItem(int value, string itemName)
    {
        BaseManager.Data.SetItemData[value] = BaseManager.Data.ItemData[itemName];
    }

    public void DisableItem(int value)
    {
        BaseManager.Data.SetItemData[value] = null;
    }

    public int LevelMaxCount(Info info)
    {
        return (info.Level + 1) * 5;
    }

    public bool SetItemCheck(string itemName)
    {
        for (int i = 0; i < BaseManager.Data.SetItemData.Length; i++)
        {
            if (BaseManager.Data.SetItemData[i] == null) { continue; } // null일 경우는 제외

            if (BaseManager.Data.SetItemData[i].name == itemName)
            {
                return true;
            }
        }
        return false;
    }
}
