using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Offline : BaseUI
{
    double moneyValue;
    [SerializeField] private TextMeshProUGUI moneyCount;
    [SerializeField] private TextMeshProUGUI TimerText;

    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part ItemPart;
    Dictionary<string, Info> items = new Dictionary<string, Info>();    

    public override bool Init()
    {
        double timeValue = Utils.TimerCheck();
        timeValue = Math.Min(timeValue, 7200);

        moneyValue = (Utils.DesignData.stageData.MONEY() * timeValue) / 3;
        moneyCount.text = StringMethod.ToCurrencyString(moneyValue);

        TimeSpan span = TimeSpan.FromSeconds(timeValue);
        TimerText.text = span.Hours + ":" + span.Minutes;

        Item_Collect(timeValue);

        foreach(var item in items)
        {
            var go = Instantiate(ItemPart, Content);
            Item_Scriptable itemData = BaseManager.Data.ItemData[item.Key];
            go.Init(itemData, item.Value.Count);
        }

        return base.Init();
    }

    private void Item_Collect(double timeValue)
    {
        int value = (int)(timeValue / 3);

        for (int i = 0; i < value; i++)
        {
            var getItems = BaseManager.Item.GetDropSet();

            for (int j = 0; j < getItems.Count; j++)
            {
                if (items.ContainsKey(getItems[j].name))
                {
                    items[getItems[j].name].Count++;
                }
                else
                {           
                    Info iteminfo = new Info();
                    iteminfo.Count++;

                    items.Add(getItems[j].name, iteminfo);                    
                }
            }
        }
    }
    public void CollectButton()
    {
        DataManager.gameData.Money += moneyValue;
        foreach(var item in items)
        {
            BaseManager.Inventory.GetItem(item.Key, item.Value.Count);
        }

        DisableOBJ();
    }

    public void ADSCollectButton()
    {
        BaseManager.ADS.ShowRewardedAd(() =>
        {
            DataManager.gameData.Money += (moneyValue*2);
            foreach (var item in items)
            {
                BaseManager.Inventory.GetItem(item.Key, item.Value.Count * 2);
            }
        });

        DisableOBJ();
    }



    public override void DisableOBJ()
    { 

        base.DisableOBJ();
    }
}
