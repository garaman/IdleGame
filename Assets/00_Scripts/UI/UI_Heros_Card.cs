using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heros_Card : MonoBehaviour
{
    [SerializeField] private Image m_slider;
    [SerializeField] private Image m_character;    
    [SerializeField] private Image m_rarity;    

    [SerializeField] private TextMeshProUGUI m_Level;
    [SerializeField] private TextMeshProUGUI m_Count;

    public void Initalize(Character_Scriptable data)
    {
        m_character.sprite = Utils.Get_Atlas(data.m_Character_Name + " 1");
        m_rarity.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
               
        m_character.SetNativeSize();
        RectTransform rect = m_character.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 4.5f, rect.sizeDelta.y / 4.5f);
    }
}
