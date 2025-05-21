using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemScriptable data;
    public int count;
}

public class ItemManager 
{
    Dictionary<string, ItemScriptable> ItemDatas = new Dictionary<string, ItemScriptable>();

    public void Init()
    {
        var datas= Resources.LoadAll<ItemScriptable>("Scriptable/Item");
        
        for(int i = 0; i < datas.Length; i++)
        {
            ItemDatas.Add(datas[i].name, datas[i]);            
        }
    }

    public List<ItemScriptable> GetDropSet()
    {
        List<ItemScriptable> dropSet = new List<ItemScriptable>();

        foreach (var item in ItemDatas)
        {
            float valueCount = Random.Range(0f, 100.0f);
            if (valueCount <= item.Value.ItemChance)
            {
                dropSet.Add(item.Value);
            }
        }

        return dropSet;
    }
}
