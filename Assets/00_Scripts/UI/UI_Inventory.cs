using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : BaseUI
{
    public enum InventoryState { ALL, EQUIPMENT, CONSUMABLE, OTHER}
    [SerializeField] InventoryState m_State = InventoryState.ALL;
    [SerializeField] RectTransform TopContent;
    [SerializeField] RectTransform m_Bar;
    [SerializeField] Button[] m_TopButtons;

    [SerializeField] Transform content;
    [SerializeField] UI_Inventory_Part part;

    [SerializeField] TextMeshProUGUI moneyText;

    public override bool Init()
    {
        var sort_Dictionary = BaseManager.Data.ItemData.OrderByDescending(x => x.Value.ItemRarity);

        foreach (var item in sort_Dictionary)
        {
            if (BaseManager.Data.ItemInfos[item.Key].Count > 0)
            {
                Instantiate(part, content).Init(item.Key);
            }
        }

        for(int i = 0; i < m_TopButtons.Length; i++)
        {
            int index = i;
            m_TopButtons[i].onClick.AddListener(() => ItemInventory_Check((InventoryState)index));
        }

        moneyText.text = StringMethod.ToCurrencyString(DataManager.gameData.Money);

        return base.Init();

    }

    public void ItemInventory_Check(InventoryState state)
    {
        m_State = state;
        StartCoroutine(barMove_Coroutine(m_TopButtons[(int)m_State].GetComponent<RectTransform>().anchoredPosition));
    }


    IEnumerator barMove_Coroutine(Vector2 endPos)
    {
        float current = 0.0f;
        float percent = 0.0f;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = endPos;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            m_Bar.anchoredPosition = LerpPos;

            yield return null;
        }
    }
}
