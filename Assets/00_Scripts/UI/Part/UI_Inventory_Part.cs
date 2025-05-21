using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Part : MonoBehaviour
{
    [SerializeField] private Image Rarity, Icon;
    [SerializeField] private TextMeshProUGUI Count_Text;

    public void Init(Item item)
    {
        Rarity.sprite = Utils.Get_Atlas(item.data.ItemRarity.ToString());
        Icon.sprite = Utils.Get_Atlas(item.data.name);
        Count_Text.text = item.count.ToString();

        GetComponent<PopUp_Handler>().Init(item.data);
    }
}
