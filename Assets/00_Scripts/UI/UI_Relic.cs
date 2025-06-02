using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic : BaseUI
{
    public Transform Content;
    public GameObject Card;

    private List<UI_Relic_Card> Cards = new List<UI_Relic_Card>();
    Dictionary<string, Item_Scriptable> m_ItemDic = new Dictionary<string, Item_Scriptable>();
    Item_Scriptable m_Item;

    [SerializeField] private UI_RelicInfo RelicInfo;
    public GameObject[] RelicPanelOBJ;

    public override bool Init()
    {
        GetRelicPanelCheck();

        var ItemData = BaseManager.Data.ItemData;        
        foreach (var item in ItemData)
        {
            if (item.Value.ItemType == ItemType.Equipment)
            {
                m_ItemDic.Add(item.Value.name, item.Value);
            }            
        }

        var sort_Dictionary = m_ItemDic.OrderByDescending(x => x.Value.ItemRarity);

        foreach (var data in sort_Dictionary)
        {
            var go = Instantiate(Card, Content).GetComponent<UI_Relic_Card>();
            Cards.Add(go);
            go.Initalize(data.Value, this);
        }

        for (int i = 0; i < RelicPanelOBJ.Length; i++)
        {
            int index = i;
            RelicPanelOBJ[i].GetComponent<Button>().onClick.RemoveAllListeners();
            RelicPanelOBJ[i].GetComponent<Button>().onClick.AddListener(() => SetItemButton(index));
        }

        return base.Init();
    }

    public void SetItemButton(int value)
    {
        if (RelicPanelOBJ[value].transform.GetChild(1).gameObject.activeSelf == true) { return; } // Lock이 true일 때는 클릭 불가

        BaseManager.Item.GetItem(value, m_Item.name);
        ItemInitalize();
    }

    public void ItemInitalize()
    {        
        SetClick(null);        

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].GetItemCheck();
        }

        Delegate_Holder.ClearEvents();
        BaseManager.Relic.Init();

        GetRelicPanelCheck();
    }

    public void GetRelicPanelCheck()
    {
        RelicLockCheck();

        for (int i = 0; i < RelicPanelOBJ.Length; i++)
        {
            if (BaseManager.Data.SetItemData[i] != null)
            {
                RelicPanelOBJ[i].GetComponent<Image>().color = Utils.Color_Rarity(BaseManager.Data.SetItemData[i].ItemRarity);
                RelicPanelOBJ[i].transform.GetChild(0).gameObject.SetActive(true);
                RelicPanelOBJ[i].transform.GetChild(0).GetComponent<Image>().sprite = Utils.Get_Atlas(BaseManager.Data.SetItemData[i].name);
            }
            else
            {
                RelicPanelOBJ[i].GetComponent<Image>().color = Color.white;
                RelicPanelOBJ[i].transform.GetChild(0).gameObject.SetActive(false);
            }            
        }
    }

    public void RelicLockCheck()
    {
        int stageValue = DataManager.gameData.Stage + 1;
        int stageForward = (stageValue / 10) + 1;

        int[] Open = { 3, 6, 9, 12, 15 };
        int index = 0;
        for (int i = 0; i < Open.Length; i++)
        {
            if (Open[i] <= stageForward)
            {
                index = i + 1;
                break;
            }
        }

        for (int i = 0; i < RelicPanelOBJ.Length; i++)
        {
            RelicPanelOBJ[i].transform.GetChild(1).gameObject.SetActive(true);
            if (i <= index)
            {
                RelicPanelOBJ[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void SetClick(UI_Relic_Card s_Card)
    {
        if (s_Card == null)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].LockOBJ.SetActive(false);
                Cards[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < BaseManager.Data.SetItemData.Length; i++)
            {
                var data = BaseManager.Data.SetItemData[i];
                if (data != null)
                {
                    if (data == s_Card.m_Item)
                    {
                        BaseManager.Item.DisableItem(i);
                        ItemInitalize();
                        return;
                    }
                }
            }

            m_Item = s_Card.m_Item;

            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].LockOBJ.SetActive(true);
                Cards[i].GetComponent<Outline>().enabled = false;
            }

            s_Card.LockOBJ.SetActive(false);
            s_Card.GetComponent<Outline>().enabled = true;
        }

    }

    public void GetRelicInfo(Item_Scriptable data)
    {
        Info iteminfo = BaseManager.Data.ItemInfos[data.name];
        float LevelUpCount = BaseManager.Item.LevelMaxCount(iteminfo);

        RelicInfo.gameObject.SetActive(true);

        RelicInfo.relicRarity.sprite = Utils.Get_Atlas(data.ItemRarity.ToString());
        RelicInfo.relicIcon.sprite = Utils.Get_Atlas(data.name);
        RelicInfo.relicNameText.text = data.ItemName;
        RelicInfo.relicDesText.text = "";
        RelicInfo.rarityText.text = Utils.String_Color_Rarity(data.ItemRarity) + data.ItemRarity.ToString() + "</color>";

        RelicInfo.relicLevelText.text = "Lv." + (BaseManager.Data.ItemInfos[data.name].Level + 1).ToString();
        RelicInfo.relicCountText.text = "(" + BaseManager.Data.ItemInfos[data.name].Count.ToString() + "/" + LevelUpCount.ToString() + ")";
        RelicInfo.sliderFill.fillAmount = (float)BaseManager.Data.ItemInfos[data.name].Count / LevelUpCount;

        RelicInfo.upgrade.onClick.RemoveAllListeners();
        //RelicInfo.upgrade.onClick.AddListener(() => UpgradeButton(data));
    }

    public override void DisableOBJ()
    {
        MainUI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
