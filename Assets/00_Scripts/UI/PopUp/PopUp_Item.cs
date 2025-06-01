using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp_Item : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI TitleText, RarityText, DescriptionText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BaseCanvas.instance.popUp = null;
            Destroy(this.gameObject);
        }
    }

    public void Item_PopUp(Item_Scriptable item, Vector2 pos)
    {
        rect.pivot = PivotPoint(pos);
        rect.anchoredPosition = pos;
        IconImage.sprite = Utils.Get_Atlas(item.name);
        TitleText.text = item.ItemName;
        RarityText.text = Utils.String_Color_Rarity(item.ItemRarity) + item.ItemRarity.ToString()+ "</color>";
        DescriptionText.text = item.ItemDescription;
    }

    public Vector2 PivotPoint(Vector2 pos)
    {
        float xPos = pos.x > Screen.width/2 ? 1.0f : 0.0f;
        float yPos = pos.y > Screen.height/2 ? 1.0f : 0.0f;

        return new Vector2(xPos, yPos);
    }
}

