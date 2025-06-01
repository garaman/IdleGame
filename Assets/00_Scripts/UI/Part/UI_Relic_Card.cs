using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic_Card : MonoBehaviour
{
    [SerializeField] private Image m_slider;
    [SerializeField] private Image m_itemIcon;    
    [SerializeField] private Image m_rarity;    

    [SerializeField] private TextMeshProUGUI m_Level;
    [SerializeField] private TextMeshProUGUI m_Count;

    [SerializeField] public GameObject LockOBJ;
    [SerializeField] public GameObject GetOBJ;
    [SerializeField] public Button onClickButton;

    [SerializeField] private GameObject plus, minus;
    [SerializeField] public Animator Effect;

    [HideInInspector] public Item_Scriptable m_Item;
    UI_Relic m_parent;

    public void Initalize(Item_Scriptable data, UI_Relic parentBase)
    {        
        m_parent = parentBase;
        m_Item = data;
        
        int levelCount = (BaseManager.Data.ItemInfos[data.name].Level+1) * 5;
        m_slider.fillAmount = (float)BaseManager.Data.ItemInfos[data.name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.ItemInfos[data.name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.ItemInfos[data.name].Level+1).ToString();

        m_itemIcon.sprite = Utils.Get_Atlas(data.name);
        m_rarity.sprite = Utils.Get_Atlas(data.ItemRarity.ToString());

        m_itemIcon.SetNativeSize();
        RectTransform rect = m_itemIcon.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);

        GetItemCheck();
    }

    public void TextCheck()
    {
        int levelCount = (BaseManager.Data.ItemInfos[m_Item.name].Level + 1) * 5;
        m_slider.fillAmount = (float)BaseManager.Data.ItemInfos[m_Item.name].Count / (float)levelCount;
        m_Count.text = BaseManager.Data.ItemInfos[m_Item.name].Count.ToString() + " / " + levelCount.ToString();
        m_Level.text = "Lv." + (BaseManager.Data.ItemInfos[m_Item.name].Level + 1).ToString();
    }
    public void EffectStart()
    {
        Effect.SetTrigger("isUpgrade");
    }

    public void GetItemCheck()
    {
        bool isGet = false;
        
        for (int i = 0; i < BaseManager.Data.SetItemData.Length; i++)
        {
            if (BaseManager.Data.SetItemData[i] != null)
            {
                if (BaseManager.Data.SetItemData[i] == m_Item)
                {
                    isGet = true;
                }
            }
        }
        
        GetOBJ.SetActive(isGet);
        plus.SetActive(!isGet);
        minus.SetActive(isGet);
    }

    
    public void ClickButton()
    {        
        m_parent.SetClick(this);        
    }

    
    public void OnClickRelicCard()
    {
        m_parent.GetRelicInfo(m_Item);
    }
    
}
