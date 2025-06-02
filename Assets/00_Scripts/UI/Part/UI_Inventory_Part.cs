using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Part : MonoBehaviour
{
    [SerializeField] private Image Rarity, Icon;
    [SerializeField] private TextMeshProUGUI Count_Text;

    public void Init(string name)
    {
        Info iteminfo = BaseManager.Data.ItemInfos[name];
        Item_Scriptable itemdata = BaseManager.Data.ItemData[name];

        Rarity.sprite = Utils.Get_Atlas(itemdata.ItemRarity.ToString());
        Icon.sprite = Utils.Get_Atlas(itemdata.name);
        Count_Text.text = iteminfo.Count.ToString();

        GetComponent<PopUp_Handler>().Init(itemdata);
    }

    public void Init(Item_Scriptable itemdata, int count)
    {
        Rarity.sprite = Utils.Get_Atlas(itemdata.ItemRarity.ToString());
        Icon.sprite = Utils.Get_Atlas(itemdata.name);
        Count_Text.text = count.ToString();

        GetComponent<PopUp_Handler>().Init(itemdata);
    }
}
